using Fairwood.Math;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class WormController : MonoBehaviour
{
    /// <summary>
    /// 单击临界距离,单位毫米
    /// </summary>
    public float CriticalClickDistanceInMillimeter = 3;
    /// <summary>
    /// 单击临界时间，单位秒
    /// </summary>
    public float CriticalClickTimeInterval = 0.4f;
    /// <summary>
    /// 中心断丝按钮范围的半径
    /// </summary>
    public float BtnBreakRadiusViewport = 0.2f;

    private const float Inch2Mm = 25.4f;
    private float _criticalClickDistanceInPixel;

    private void Awake()
    {
        _criticalClickDistanceInPixel = CriticalClickDistanceInMillimeter*(Screen.dpi*Inch2Mm);
    }

    public Worm WormToControl;
    public Camera ControlCamera;
    /// <summary>
    /// 下一帧中断操作，暂停游戏时用到，防止跳点
    /// </summary>
    public bool CancelControlNextFrame = false;

    /// <summary>
    /// 上一帧鼠标位置(px)
    /// </summary>
    private Vector2 _lastTouchPos;

    private TouchState _lastTouchState;

    private ControlState _controlState;
    private Vector2 _startControlPos;//按下时的位置，用于判断是否距离超过临界值
    private float _startControlRealTime;//按下时的时间，用于判断是否时间超过临界值
    private void Update()
    {
        if (CancelControlNextFrame)//如果终止操纵，则当做上一帧不在操纵状态
        {
            _lastTouchState = TouchState.Off;
            _controlState = ControlState.NotControl;
            CancelControlNextFrame = false;
        }

        Vector2 curTouchPos;
        TouchState curTouchState;

        #region 获取操纵参数

        if (Input.GetKey(KeyCode.Mouse0)) //有鼠标按下
        {
            curTouchPos = Input.mousePosition; //鼠标为操控者
            curTouchState = Input.touchCount <= 0 ? TouchState.Touched : TouchState.MoreThanOne; //是否还有别的触摸
        }
        else //没有鼠标
        {
            if (Input.touchCount == 0) //也没有触摸
            {
                curTouchPos = Vector2.zero; //给个默认值，不会用到的
                curTouchState = TouchState.Off;
            }
            else
            {
                curTouchPos = Input.touches[0].position;
                curTouchState = Input.touchCount == 1 ? TouchState.Touched : TouchState.MoreThanOne;
            }
        }

        #endregion

        #region 检测操纵有效性

        if (curTouchState == TouchState.Touched)
        {
            //todo:怎样正确的获得鼠标事件，而不是这样不顾前面的接收器遮挡
            var ray = ControlCamera.ScreenPointToRay(curTouchPos);
            Debug.DrawRay(ray.origin, ray.direction * ControlCamera.farClipPlane, Color.red);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit, ControlCamera.farClipPlane) || hit.collider != collider)
                //如果鼠标没按到东西或者按到的不是我，就设为未按着，需优化
            {
                curTouchState = TouchState.Off;
            }
        }

        #endregion

        #region 更新操纵状态

        if (_controlState == ControlState.NotControl) //这一帧开始操作，下一帧才能看到效果
        {
            if (curTouchState == TouchState.Touched)
            {
                _controlState = ControlState.JustStartControl;
                _startControlPos = curTouchPos;
                _startControlRealTime = Time.realtimeSinceStartup;
            }
        }
        else if (_controlState == ControlState.JustStartControl) //上一帧处于刚开始操作状态
        {
            if (curTouchState == TouchState.Touched)
            {
                ProcessDrag(_lastTouchPos, curTouchPos); //处理伸缩操作

                if (Vector2.Distance(curTouchPos, _startControlPos) > CriticalClickDistanceInMillimeter
                    || Time.realtimeSinceStartup - _startControlRealTime > CriticalClickTimeInterval)
                    //是否超过临界，进入Drag状态，不再可能是单击了
                {
                    _controlState = ControlState.Drag;
                }
            }
            else if (curTouchState == TouchState.Off) //那就是单击了
            {
                ProcessClick(_startControlPos); //单击事件：吐丝/断丝

                _controlState = ControlState.NotControl;
            }
            else // curTouchState == TouchState.MoreThanOne//双指模式
            {
                _controlState = ControlState.NotControl;
            }
        }
        else if (_controlState == ControlState.Drag)
        {
            if (curTouchState == TouchState.Touched)
            {
                ProcessDrag(_lastTouchPos, curTouchPos); //处理伸缩操作
            }
            else //终止操作
            {
                _controlState = ControlState.NotControl;
            }
        }

        if (_controlState != ControlState.NotControl)
        {
            _lastTouchPos = curTouchPos;
        }
        _lastTouchState = curTouchState;

        #endregion

        if (_controlState == ControlState.JustStartControl || _controlState == ControlState.Drag)
        {
            var dL = (curTouchPos - _lastTouchPos).y*ControlCamera.orthographicSize*2/Screen.height;
            WormToControl.LengthenSilk(dL);
        }
    }

    void ProcessDrag(Vector2 lastPosPx, Vector2 curPosPx)
    {
        if (curPosPx.y == lastPosPx.y) return;//没有垂直分量，不伸缩
        var direction = curPosPx.y > lastPosPx.y ? 1 : -1;//1表示伸长，-1表示缩短
        float dl;//伸长量
        if (ControlCamera.orthographic)
        {
            dl = (curPosPx.y - lastPosPx.y)/ControlCamera.pixelHeight*ControlCamera.orthographicSize*2;
        }
        else
        {
            //用射线探测法获得dl
            var ray0 = ControlCamera.ScreenPointToRay(lastPosPx);
            var ray1 = ControlCamera.ScreenPointToRay(curPosPx);
            var plane = new Plane(Vector3.back, WormToControl.transform.position);//关卡一定是朝向back方向的
            float enter0, enter1;
            if (plane.Raycast(ray0, out enter0) && plane.Raycast(ray1, out enter1))
            {
                var p0 = ray0.GetPoint(enter0);
                var p1 = ray1.GetPoint(enter1);
                dl = p1.y - p0.y;
            }
            else
            {
                return;
            }
        }
        WormToControl.LengthenSilk(dl);
    }

    void ProcessClick(Vector2 clickPosPx)
    {
        Debug.Log("ProcessClick");
        //根据范围判断是吐丝还是断丝
        var clickPosViewport = ControlCamera.ScreenToViewportPoint(clickPosPx);
        if (new Vector2(clickPosViewport.x - 0.5f, clickPosViewport.y - 0.5f).magnitude > BtnBreakRadiusViewport)//圈外，吐丝
        {
            //用射线探测法获得射出点，再算出方向
            var ray = ControlCamera.ScreenPointToRay(clickPosPx);
            var plane = new Plane(Vector3.back, WormToControl.transform.position);//关卡一定是朝向back方向的
            float enter0, enter1;
            if (plane.Raycast(ray, out enter0))
            {
                var p = ray.GetPoint(enter0);
                Vector2 dir = p.ToVector2() - WormToControl.MouthPosWorld;
                WormToControl.Spit(dir);
            }
        }
        else//圈内，断丝
        {
            WormToControl.BreakUpHeldSilk();
        }
    }

    /// <summary>
    /// 包括鼠标
    /// </summary>
    enum TouchState
    {
        Off,
        Touched,
        MoreThanOne,
    }
    enum ControlState
    {
        NotControl,
        /// <summary>
        /// 此阶段抬起将是单击
        /// </summary>
        JustStartControl,
        /// <summary>
        /// 超过临界距离或时间
        /// </summary>
        Drag,
    }
}
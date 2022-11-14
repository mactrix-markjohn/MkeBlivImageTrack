using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndRotateCube : MonoBehaviour
{
    public bool isActive = false;
   // Color activeColor = new Color();
    public GameObject sphereVideo;
    public float speed = 0.05f ;


    private float m_startTime  = 0.0f;
    private Vector2 m_startPos = Vector2.zero;

    private bool m_swiping = false;
    private bool m_holding = false;
    private float m_minDist  = 50.0f;
    private float m_maxTime = 0.5f;

    enum DragDirection
    {
        Right,
        Left,
        Up,
        Down
    }

    private DragDirection dragDirection;
    
 
    // Start is called before the first frame update
    void Start()
    {
        dragDirection = DragDirection.Right;
    }
 
    // Update is called once per frame
    void Update()
    {
        
        
        if (isActive)
        {
         
            
            if (Input.touchCount > 0){
                foreach (Touch screenTouch in Input.touches) {
               
                    switch (screenTouch.phase) {
                        
                        case TouchPhase.Began :
                            
                            m_swiping = true;
                            m_startTime = Time.time;
                            m_startPos = screenTouch.deltaPosition;
                            break;
                        
                        case TouchPhase.Canceled :
                            
                            m_holding = false;
                            m_swiping = false;
                            break;
                        
                        case TouchPhase.Ended :
                            
                            isActive = false;
                            m_holding = false;
                            break;
                        
                        case TouchPhase.Moved :
                            
                            TouchMoved(screenTouch);
                            break;
                        
                        case TouchPhase.Stationary :
                            
                            //DetectSwipe(screenTouch);
                            break;
                
                    }
                }
            }
            
        }
        else
        {
           //Do something
        }
 
       
    }

    private void TouchMoved(Touch screenTouch)
    {

        dragDirection = DetectDrag(screenTouch);
        
        
        /*float xDirection = screenTouch.deltaPosition.x * speed;
        //sphereVideo.transform.Rotate(0f, xDirection, 0f);

        float yDirection = screenTouch.deltaPosition.y * speed;
        sphereVideo.transform.Rotate(yDirection, xDirection, 0f);*/
    }


    DragDirection DetectDrag(Touch screenTouch)
    {

        Vector2 direction = screenTouch.deltaPosition - m_startPos;
        Vector2 swipeType = Vector2.zero;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Horizontal
            swipeType = Vector2.right * Mathf.Sign(direction.x);
        }
        else
        {
            //Vertical
            swipeType = Vector2.up * Mathf.Sign(direction.y);
        }

        if (swipeType.x != 0.0f)
        {
            m_holding = true;

            if (swipeType.x > 0.0f)
            {
                // Handle right drag
                
                float xDirection = screenTouch.deltaPosition.x * speed;
                sphereVideo.transform.Rotate(0f, xDirection, 0f);
                
                return DragDirection.Right;
            }
            else
            {
                // Handle left drag
                
                float xDirection = screenTouch.deltaPosition.x * speed;
                sphereVideo.transform.Rotate(0f, xDirection, 0f);
                
                return DragDirection.Left;
            }
        }

        if (swipeType.y != 0.0f)
        {
            m_swiping = false;
            if (swipeType.y > 0.0f)
            {
                // handle up drag
                
                float yDirection = screenTouch.deltaPosition.y * speed;
                sphereVideo.transform.Rotate(0f, 0f, yDirection);
                
                
                return DragDirection.Up;
            }
            else
            {
                // handle down drag
                
                float yDirection = screenTouch.deltaPosition.y * speed;
                sphereVideo.transform.Rotate(0f, 0f, yDirection);
                
                return DragDirection.Down;
            }
        }

        return DragDirection.Right;

    }

    void DetectSwipe (Touch touch) {
       
        float totalTime = Time.time - m_startTime;
        float totalDist = (touch.position - m_startPos).magnitude;
        
        if (m_holding || (m_swiping && totalTime < m_maxTime && totalDist > m_minDist)) {
            
            Vector2 direction = touch.position - m_startPos;
            Vector2 swipeType = Vector2.zero;
        
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
                // Horizontal:
                swipeType = Vector2.right * Mathf.Sign(direction.x);
            }
            else{
                // Vertical:
                swipeType = Vector2.up * Mathf.Sign(direction.y);
            }

            if(swipeType.x != 0.0f){
                m_holding = true;
               
                if(swipeType.x > 0.0f){
                    // HANDLE RIGHT SWIPE
                }
                else{
                    // HANDLE LEFT SWIPE
                }
                 
            }
            
            if(swipeType.y != 0.0f ){
                m_swiping = false;  // <- THIS MAKES THE DIFFERENCE
            
                if(swipeType.y > 0.0f){
                    // HANDLE UP SWIPE
                }
                else{
                    // HANDLE DOWN SWIPE
                }
            }
        }
    }
}

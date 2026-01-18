using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private State currentState;
    
    public void SwitchState(State newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentState?.Tick(Time.deltaTime); // Null conditional operator '?'
    }
    
}
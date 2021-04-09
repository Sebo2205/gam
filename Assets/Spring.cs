using UnityEngine;
public class Spring : MonoBehaviour {
    public float force;
    public bool resetVelocity = true;
    public float previewGravityMultiplier = 1.5f;
    float cooldown = 0;
    void Update() {
        if (cooldown > 0) cooldown -= Time.deltaTime;
    }
    void OnTriggerEnter2D(Collider2D col) {
        if (cooldown > 0) return;
        Debug.Log("Collision lol");
        Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
        Player p = col.GetComponent<Player>();
        if (p) {
            p.stomp = false;
            p.spring = true;
            p.jump = false;
            p.timer = .6f;
        }
        if (rb) {
            rb.position = transform.position;
            if (resetVelocity) rb.velocity = transform.up * force;
            else rb.AddForce(transform.up * force);
            cooldown = .1f;
        }
    }
    // Source: http://answers.unity.com/answers/296865/view.html
    Vector3 PlotTrajectoryAtTime (Vector3 start, Vector3 startVelocity, float time) {
        return start + startVelocity*time + (Physics.gravity * previewGravityMultiplier)*time*time*0.5f;
    }
    void PlotTrajectory (Vector3 start, Vector3 startVelocity, float timestep, float maxTime) {
        Vector3 prev = start;
        for (int i=1;;i++) {
            float t = timestep*i;
            if (t > maxTime) break;
            Vector3 pos = PlotTrajectoryAtTime (start, startVelocity, t);
            Debug.DrawLine (prev,pos,Color.red);
            prev = pos;
        }
    }
    void OnDrawGizmos() {
        PlotTrajectory(transform.position, transform.up * force, .12f, 5);
    }
}
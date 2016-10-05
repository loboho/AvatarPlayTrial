using UnityEngine;
using System.Collections;

public class TwardsTo : MonoBehaviour {
    public Transform[] targets;
    public float rotaSpeed = 2f;
    public bool ignoreY;
    BH.AvPlayer avPlayer;

    int curTarget;
	// Use this for initialization
	void Start () {
        avPlayer = GetComponent<BH.AvPlayer>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        var target = targets[curTarget];
        if (!target)
            return;

        var targetDir = (target.position - transform.position);
        if (ignoreY)
            targetDir.y = 0;
        var targetDisSqr = targetDir.sqrMagnitude;
        if (targetDisSqr < 1f)
        {
            curTarget = (curTarget + 1) % targets.Length;
            return;
        }

        if (avPlayer && avPlayer.layer[0].curRTClip != null)
        {
            if (avPlayer.layer[0].curRTClip.clipName == "Dive")
            {
                var clip = avPlayer.layer[0].curRTClip.clip;
                var posi1 = clip.GetNamedFramePosi("StartJump");
                var posi2 = clip.GetNamedFramePosi("EndJump");
                var curPosi = avPlayer.layer[0].curPosi;
                if (curPosi < posi2 && curPosi > posi1) // don't do rotate when jump
                    return;
            }
        }


        targetDir = (target.position - transform.position);
        if (ignoreY)
        {
            var curDir = transform.forward;
            var horDisSqr = curDir.x * curDir.x + curDir.z * curDir.z;
            if (horDisSqr > 0)
            {
                var ySqrUponHorSqr = curDir.y * curDir.y / horDisSqr;
                var targetHorDisSqr = targetDir.x * targetDir.x + targetDir.z * targetDir.z;
                targetDir.y = Mathf.Sqrt(targetHorDisSqr * ySqrUponHorSqr);
            }
        }

        var rotateToDir = Vector3.RotateTowards(transform.forward, targetDir.normalized, rotaSpeed * Time.deltaTime, 1f);

        transform.forward = rotateToDir;
    }
}

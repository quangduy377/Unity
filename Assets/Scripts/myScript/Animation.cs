using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation
{
    public static void fire(ref Animator anim)
    {
        anim.SetBool("firing", true);
    }

    public static void fireToRun(ref Animator anim)
    {
        anim.SetBool("running", true);
        anim.SetBool("firing", false);

    }
    public static void runToMerge(ref Animator anim)
    {
        anim.SetBool("merge", true);
        anim.SetBool("attacking", true);
    }
    public static void attackToRun(ref Animator anim)
    {
        anim.SetBool("running", true);
        anim.SetBool("attacking", false);
    }
    public static void runToAttack(ref Animator anim)
    {
        anim.SetBool("attacking", true);
        anim.SetBool("merge", false);
    }

    public static void dead(ref Animator anim)
    {
        anim.SetBool("dead", true);
        anim.SetBool("running", false);
        anim.SetBool("attacking", false);
    }
}

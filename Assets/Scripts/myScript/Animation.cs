using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation
{
    public static void runToMerge(ref Animator anim)
    {
        anim.SetBool("running", false);
        anim.SetBool("merge", true);
    }
    public static void mergeToRun(ref Animator anim)
    {
        anim.SetBool("running", true);
        anim.SetBool("merge", false);
    }

    public static void runToAttack(ref Animator anim)
    {
        anim.SetBool("running", false);
        anim.SetBool("attacking", true);
    }
    public static void attackToRun(ref Animator anim)
    {
        anim.SetBool("running", true);
        anim.SetBool("attacking", false);
    }
    public static void attackToDead(ref Animator anim)
    {
        anim.SetBool("dead", true);
        anim.SetBool("attack", false);
    }
}

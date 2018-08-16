using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherStats{
    
    public static ArcherContainer GetArcherStats1 { get { return new ArcherContainer(15f, 10, 5f,1); } }
    public static ArcherContainer GetArcherStats2 { get { return new ArcherContainer(15f, 15, 4f,2); } }
    public static ArcherContainer GetArcherStats3 { get { return new ArcherContainer(15f, 20, 3f,3); } }
}

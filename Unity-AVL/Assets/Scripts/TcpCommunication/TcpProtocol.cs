using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TcpProtocol {
    public const string CONNECTION_INIT_UNITY = "CONNECTION_INIT_UNITY";

    public const string UNITY_DELT_CAR = "UNITY_DELT_CAR";
    public const string UNITY_INIT_CAR = "UNITY_INIT_CAR";
    public const string UNITY_UPDT_CAR = "UNITY_UPDT_CAR";

    public const string UNITY_INIT_JUNC = "UNITY_INIT_JUNC";
    public const string UNITY_INIT_EDGE = "UNITY_INIT_EDGE";

    public const string SUMO_INIT_LISTENER = "SUMO_INIT_LISTENER";
    public const string SUMO_INIT_EGO = "SUMO_INIT_EGO";
    public const string SUMO_UPDT_EGO = "SUMO_UPDT_EGO";

    public const string TO_UNITY = "TO_UNITY";
    public const string TO_SUMO = "TO_SUMO";

    public const char MSG_DELIM = '$';
    public const char DATA_DELIM = '|';
}

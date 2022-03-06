using System;

namespace UE_VSAssetHelper.UE
{
    enum RequestType
    {
        GET_INFO,
        OPEN
    }

    enum ResponseStatus
    {
        OK,
        ERROR
    }

    struct IDEResponse
    {
        public ResponseStatus status;
        public String answerString;
    }

    struct IDERequest
    {
        public RequestType type;
        public String data;
    }
    struct BlueprintClassObject
    {
        public int Index;
        public String ObjectName;
        public String ClassName;
        public String SuperClassName;
        public String PackageName;
    }
}

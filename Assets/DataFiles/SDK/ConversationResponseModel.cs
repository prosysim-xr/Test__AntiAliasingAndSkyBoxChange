using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sdk
{
    [System.Serializable]
    public class ConversationResponseModel
    {
        public string name;
        public string type;
        public int wait;
        public string title;
        public string ui_element;
        public string placeholder;
        public string state_options;
        public string[] options;
        public DataModel data;
        public string engagement_id;
        public ResponseChannels response_channels;
        [System.Serializable]
        public class DataModel
        {
            //"data": {"fbx_path": "", "scene_to_load": "S2", "voice_audio_path": "https://general-iamdave.s3-us-west-2.amazonaws.com/hi-i-am-dave-welcome1579176373.mp3"}
            public string fbx_path;
            public string scene_to_load;
            public string voice_audio_path;

        }
        [System.Serializable]
        public class ResponseChannels
        {
            public string icon;
            public string voice;
            public string threed;
            public string mouth;


        }
    }

}

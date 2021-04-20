using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;

namespace Example1
{

    public class Example1Program : MonoBehaviour
    {
        [SerializeField]
        private string hostName = "localhost";
        [SerializeField]
        private int port = 8000;
        [SerializeField]
        private string clientName = "Example 1";

        private AudioStreamer audioStreamer;
        private IEnumerator streamMessagesCoroutine;

        private bool stopStreaming = false;

        // Start is called before the first frame update
        void Start()
        {
            // The AudioStreamer class provides a client (chat) for AudioStreamer
            // (https://github.com/agektmr/AudioStreamer).

            audioStreamer = new AudioStreamer($"ws://{hostName}:{port}/socket");
            audioStreamer.Connect(clientName);

            streamMessagesCoroutine = StreamMessages();
            StartCoroutine(streamMessagesCoroutine);
        }

        IEnumerator StreamMessages()
        {
            while (true)
            {
                if (audioStreamer.ReadyState == WebSocketSharp.WebSocketState.Open)
                {
                    audioStreamer.Write($"{Time.time}: lalala");
                }
                else
                {
                    Debug.Log($"{Time.time} - WebSocketState: {audioStreamer.ReadyState}");
                }

                lock (this)
                {
                    if (stopStreaming)
                    {
                        break;
                    }
                }

                yield return new WaitForSeconds(5);
            }
            yield return null;
        }

        void OnDestroy()
        {
            lock (this)
            {
                stopStreaming = true;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace DuelMasters {
    public class Deck : MonoBehaviour {
        public Card[] Cards;

        public int Foo => 1;

        public async Task Asyn() {

        }

        public Action Bar() {
            
            return () => {
                for (int i = 0; i < 5; i++) {
                    Debug.Log($"{i} works");
                }
            };
        }

        private void Start() {
            Action a = Bar();
            Debug.Log("a -- Start");
            a();
            Debug.Log("a -- End");
        }
        

    }
}
    
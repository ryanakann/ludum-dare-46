using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buffers {
    public enum BufferedBoolType {
        AllTrue,
        AllFalse,
        AnyTrue,
        AnyFalse,
    }

    public class BufferedBool : MonoBehaviour {
        private int bufferSize;
        private int buffer;
        private BufferedBoolType type;

        public BufferedBool (int bufferSize = 4, BufferedBoolType type = BufferedBoolType.AnyTrue) {
            this.bufferSize = bufferSize;
            this.type = type;
        }

        public void UpdateBool (bool pressed) {
            if (pressed) {
                buffer = bufferSize;
            } else {
                if (buffer > 0) {
                    buffer--;
                }
            }
        }

        public bool IsTrue () {
            return buffer > 0;
        }
    }
}
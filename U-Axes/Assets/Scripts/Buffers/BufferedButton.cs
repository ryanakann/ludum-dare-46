using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buffers {
    public class BufferedButton {
    private string buttonName;
    private int bufferSize;
    private int bufferDown;
    private int bufferStay;
    private int bufferUp;

    public BufferedButton (string buttonName, int bufferSize = 4) {
        this.buttonName = buttonName;
        this.bufferSize = bufferSize;
    }

    public void UpdateButton () {
        if (Input.GetButtonDown(buttonName)) {
            bufferDown = bufferSize;
        } else {
            if (bufferDown > 0) {
                bufferDown--;
            }
        }

        if (Input.GetButton(buttonName)) {
            bufferStay = bufferSize;
        } else {
            if (bufferStay > 0) {
                bufferStay--;
            }
        }

        if (Input.GetButtonUp(buttonName)) {
            bufferUp = bufferSize;
        } else {
            if (bufferUp > 0) {
                bufferUp--;
            }
        }
    }

    public bool Down () {
        return bufferDown > 0;
    }

    public bool Stay () {
        return bufferStay > 0;
    }

    public bool Up () {
        return bufferUp > 0;
    }
}
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace MouseO
{
    public class Btn {
        public bool clicked;
        public bool released;
        public bool state;
        public Btn(bool c, bool r, bool s) {
            clicked = c;
            released = r;
            state = s;
        }
    }
    public class MouseO
    {
        List<bool> _down_last;
        List<bool> _down_last2;
        public MouseO() {
            _down_last = new List<bool>() {false, false, false, false, false};
            _down_last2 = new List<bool>() {false, false, false, false, false};
        }
        public List<Btn> update(List<bool> btns) {
            List<Btn> o = new();
            for (int i = 0; i < btns.Count; i++) {
                _down_last[i] = btns[i];
                bool c = false;
                if (btns[i] && !_down_last2[i]) {
                    c = true;
                }
                o.Add(new Btn(c, _down_last2[i] && !btns[i], btns[i]) );
                _down_last2[i] = btns[i];
            }
            return o;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;

namespace Common.Data
{
    public class CharacterDefine
    {
        public int TID { get; set; }
        public string Name { get; set; }
        public CharacterClass Class { get; set; }
        public string Resource { get; set; }
        public string Description { get; set; }
        public float Height { get; set; }
        //基本属性
        public int Speed { get; set; }
        //生命
        public float MaxHP { get; set; }
        //法力
        public float MaxMP { get; set; }
        //力量成长
        public float GrowthSTR { get; set; }
        //智力成长
        public float GrowthINT { get; set; }
        public float GrowthDEX { get; set; }
        //力量
        public float STR { get; set; }
        //智力
        public float INT { get; set; }
        public float DEX { get; set; }
        public float AD { get; set; }
        public float AP { get; set; }
        public float DEF { get; set; }
        public float MDEF { get; set; }
        public float SPD { get; set; }
        public float CRI { get; set; }
        public string AI { get; set; }
    }
}

using UnityEngine;
using System.Collections;

    public class Talent {
        public int id; //用来设置外部关系的主键
        public string spendPoint; //确定当前节点学习耗费的点数 初始学习点数,每级耗费点数,是否累加  5，2，1
        public int maxPileLevel; //最大可堆叠级别，级别决定最大堆叠点数。为了减少配置错误，通过消耗数值自动计算最大堆叠点数。
        public int preTotalPoint; //确定当前节点可用的前置节点的总点数
        public string preNodeLevel;//确定当前节点可用的某前置节点的已堆叠点数 row,column,alreadyPileLevel
        public string nextSockets; //下一接口索引 1表示连接,0表示不连
        public string codition; //额外确定当前节点可用的非内置条件。例如：角色等级，HP，力量，攻击力，等属性
    }

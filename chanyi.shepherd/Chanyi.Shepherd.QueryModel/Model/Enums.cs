using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel
{

    public enum GenderEnum
    {
        Male, Female
    }

    /// <summary>
    /// 生理阶段(成长阶段)
    /// </summary>
    public enum GrowthStageEnum
    {
        /// <summary>
        /// 种羊
        /// </summary>
        StudSheep,
        /// <summary>
        /// 羔羊
        /// </summary>
        Lamb,
        /// <summary>
        /// 育成羊
        /// </summary>
        LambHog,
        /// <summary>
        /// 育肥羊
        /// </summary>
        FattingSheep,
        /// <summary>
        /// 后备种羊(第二次鉴定后)
        /// </summary>
        TemporaryStudSheep
    }

    /// <summary>
    /// 羊只来源
    /// </summary>
    public enum OriginEnum
    {
        /// <summary>
        /// 购入
        /// </summary>
        Purchase,
        /// <summary>
        /// 自繁
        /// </summary>
        HomeBred
    }

    public enum SheepStatusEnum
    {
        Nomal,
        /// <summary>
        /// 出售
        /// </summary>
        Selled,
        /// <summary>
        /// 死亡
        /// </summary>
        Dead,
        /// <summary>
        /// 外部羊只，如购入羊只的父母，此羊只不在厂中
        /// </summary>
        Outer
    }

    /// <summary>
    /// 死亡处理方式
    /// </summary>
    public enum DeathDisposeEnum
    {
        /// <summary>
        /// 销毁
        /// </summary>
        Destroy,
        /// <summary>
        /// 其他
        /// </summary>
        Other
    }

    /// <summary>
    /// 分娩方式
    /// </summary>
    public enum DeliveryWayEnum
    {
        Normal,
        /// <summary>
        /// 助产
        /// </summary>
        Deliver
    }


    /// <summary>
    /// 助产原因
    /// </summary>
    public enum MidwiferyReasonEnum
    {
        /// <summary>
        /// 无助产原因
        /// </summary>
        None,
        /// <summary>
        /// 胎位不正
        /// </summary>
        Malposition,
        /// <summary>
        /// 阴道狭窄
        /// </summary>
        Colposynizesis,
        /// <summary>
        /// 其他
        /// </summary>
        Other
    }

    /// <summary>
    /// 投入品出入库
    /// </summary>
    public enum InOutWarehouseDirectionEnum
    {
        In,
        Out
    }

    /// <summary>
    /// 投入品出库 去向
    /// </summary>
    public enum OutWarehouseDispositonEnum
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 自己用
        /// </summary>
        SelfUse,
        /// <summary>
        /// 卖出
        /// </summary>
        Sell
    }

    public enum EmployeeStatusEnum
    {
        OnJob,

        /// <summary>
        /// 离职
        /// </summary>
        Dimission,
    }

    /// <summary>
    /// 配置项提醒
    /// </summary>
    public enum SettingsEnum
    {
        /// <summary>
        /// 预产期
        /// </summary>
        PreDeliveryRemaindful,
        /// <summary>
        /// 断奶期
        /// </summary>
        PreAblactationRemaindful,
        /// <summary>
        /// 饲料临界值
        /// </summary>
        FeedRemaindful,
        /// <summary>
        /// 药品临界值
        /// </summary>
        MedicineRemaindful
    }

    /// <summary>
    /// 季节
    /// </summary>
    public enum SeasonEnum
    {
        Spring,
        /// <summary>
        /// 秋季
        /// </summary>
        Autumn
    }

}

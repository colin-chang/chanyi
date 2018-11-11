using System;
using System.Collections.Generic;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.Filter.BaseInfo;
using Chanyi.Shepherd.QueryModel.Model.BaseInfo;
using Chanyi.Shepherd.QueryModel.Model.System;
using Chanyi.Shepherd.QueryModel.Filter.HR;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Model.Multiplying;
using Chanyi.Shepherd.QueryModel.Filter.Multiplying;
using Chanyi.Shepherd.QueryModel.BindingFilter;
using Chanyi.Shepherd.QueryModel.Model.HR;
using Chanyi.Shepherd.QueryModel.Model.Assist;
using Chanyi.Shepherd.QueryModel.Filter.Group;
using Chanyi.Shepherd.QueryModel.Model.Group;
using Chanyi.Shepherd.QueryModel.Model.Chart;
using Chanyi.Shepherd.QueryModel.Model.Formula;
using Chanyi.Shepherd.QueryModel.Filter.Formula;
using Chanyi.Shepherd.QueryModel.Model.Input;
using Chanyi.Shepherd.QueryModel.Filter.Input;
using Chanyi.Shepherd.QueryModel.Model.Finance;
using Chanyi.Shepherd.QueryModel.Filter.Finance;
using Chanyi.Shepherd.QueryModel.UpdateModel.System;
using Chanyi.Shepherd.QueryModel.Model.Breeding;
using Chanyi.Shepherd.QueryModel.Filter.Breeding;
using Chanyi.Shepherd.QueryModel.Model.DiseaseControl;
using Chanyi.Shepherd.QueryModel.Filter.DiseaseControl;
using Chanyi.Shepherd.QueryModel.AddModel.Finance;
using Chanyi.Shepherd.QueryModel.Model.ReportForms;
using Chanyi.Shepherd.QueryModel.Filter.System;

namespace Chanyi.Shepherd.IDao
{
    public interface IDal
    {
        #region Bind

        void Initialize();

        List<SheepBind> GetSheepBind(SheepBindFilter filter);

        List<SheepBind> GetSheepParentBind();

        List<SheepBind> GetSheepParentBind(string childId);

        /// <summary>
        /// 获取待流产羊绑定
        /// </summary>
        /// 母羊/种羊/近期已经交配过/交配后并未流产或者分娩
        /// <returns></returns>
        List<SheepBind> GetAbortionSheepBind(int value, int range);
        /// <summary>
        /// 获取待分娩羊绑定
        /// </summary>
        /// 母羊/种羊/近期已经交配过/交配后并未流产或者分娩
        /// <returns></returns>
        List<SheepBind> GetDeliverySheepBind(int value, int range);

        /// <summary>
        /// 配种羊只查询绑定
        /// </summary>
        /// <returns></returns>
        List<SheepBind> GetMatingSheepSelectBind();

        List<SheepBind> GetMatingSheepCountBind();

        List<SheepBind> GetStudSheepBind();

        List<SheepBind> GetStudSheepBindWithOuter();

        /// <summary>
        /// 获取已经流产的羊只绑定
        /// </summary>
        /// <returns></returns>
        List<SheepBind> GetAbortionSheepSelectBind();

        /// <summary>
        /// 获取已经分娩的羊只绑定
        /// </summary>
        /// <returns></returns>
        List<SheepBind> GetDeliverySheepSelectBind();

        /// <summary>
        /// 种羊羊只鉴定绑定
        /// </summary>
        /// <returns></returns>
        List<SheepBind> GetStudAssessSheepSelectBind();
        /// <summary>
        /// 第一次鉴定绑定
        /// </summary>
        /// <returns></returns>
        List<SheepBind> GetFirstAssessSheepSelectBind();

        /// <summary>
        /// 死亡管理绑定
        /// </summary>
        /// <returns></returns>
        List<SheepBind> GetDeathSheepSelectBind();

        /// <summary>
        /// 第二次鉴定羊只绑定
        /// </summary>
        /// 第一次鉴定过/除了羔羊、种羊
        /// <returns></returns>
        List<SheepBind> GetSecondAssessSheepAddBind();

        /// <summary>
        /// 第二次羊只鉴定查询羊只绑定
        /// </summary>
        /// <returns></returns>
        List<SheepBind> GetSecondAssessSheepSelectBind();

        /// <summary>
        /// 第三次鉴定羊只绑定
        /// </summary>
        /// 第二次鉴定过的/除了羔羊种羊
        /// <returns></returns>
        List<SheepBind> GetThirdAssessSheepAddBind();

        /// <summary>
        /// 第三次羊只鉴定查询羊只绑定
        /// </summary>
        /// <returns></returns>
        List<SheepBind> GetThirdAssessSheepSelectBind();

        List<SheepBind> GetExceptAssessSheepAddBind();
        List<SheepBind> GetExceptAssessSheepSelectBind();

        List<SheepBind> GetTreatmentSheepBind();

        List<BreedBind> GetBreedBind();

        List<SheepfoldBind> GetSheepfoldBind();

        List<SheepfoldSheepCountBind> GetSheepfoldSheepCountBind();

        List<EmployeeBind> GetEmployeeBind();

        List<EmployeeBind> GetAllEmployeeBind();

        List<UserBind> GetUserBind();

        List<DutyBind> GetDutyBind();

        Dictionary<SheepfoldBind, List<SheepBind>> GetMoveSheepfoldBind();

        Dictionary<DiseaseTypeBind, List<DiseaseBind>> GetDiseaseTypeBind();

        /// <summary>
        /// 症状ID搜索疾病（包含一种症状就算）
        /// </summary>
        /// <param name="symptomIds"></param>
        /// <returns></returns>
        List<DiseaseBind> GetDiseaseBindBySymptomIds(string[] symptomIds);

        List<DiseaseBind> GetDiseaseBindByName(string diseaseName);

        Dictionary<SymptomTypeBind, List<SymptomBind>> GetSymptomTypeBind();

        List<SymptomBind> GetSymptomBind(SymptomBindFilter filter);

        List<PurchaserBind> GetPurchaserBind();

        List<FeedNameBind> GetFeedNameBind();
        List<FeedTypeBind> GetFeedTypeBind();
        List<FeedTypeBind> GetFeedTypeBind(string feedNameId);
        List<AreaBind> GetAreaBind();
        List<AreaBind> GetAreaBind(string feedNameId, string typeId);

        List<MedicineBind> GetMedicineNameBind();
        List<ManufactureBind> GetManufactureBind();
        List<ManufactureBind> GetManufactureBind(string medicineNameId);
        List<DepartmentBind> GetDepartmentBind();
        List<DepartmentBind> GetDepartmentBind(string medicineNameId);
        List<MedicineTypeBind> GetMedicineTypeBind();

        /// <summary>
        /// 根据药品名称ID查药品类型
        /// </summary>
        /// <param name="MedicineNameId"></param>
        /// <returns></returns>
        List<MedicineTypeBind> GetMedicineTypeBind(string MedicineNameId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MedicineNameId">药品ID</param>
        /// <param name="typeId">种类ID</param>
        /// <returns></returns>
        List<CooperaterBind> GetManufacturerBind(string MedicineNameId, string typeId);

        List<OtherBind> GetOtherBind();

        List<SheepBind> GetBuySheepBind4Add();
        List<SheepBind> GetBuySheepBind4Select();

        List<BuyFeedBind> GetBuyFeedBind4Add();
        List<BuyMedicineBind> GetBuyMedicineBind4Add();
        List<BuyOtherBind> GetBuyOtherBind4Add();

        List<SellFeedBind> GetSellFeedBind4Add();
        List<SellOtherBind> GetSellOtherBind4Add();

        List<FormulaBind> GetFormulaBind(bool isEnable);

        #endregion

        #region Chart

        List<PeriodsSheepGrowthStageCount> GetPeriodsSheepGrowthStageCount(DateTime date, string breedId, GenderEnum? gender = null);

        List<PeriodsSellSheepCount> GetPeriodsSellSheepCount(DateTime dtStart, DateTime dtEnd);

        #endregion

        #region BaseInfo
        List<Sheep> GetSheep(SheepFilter filter, int rowsCount);

        List<Sheep> GetSheep(SheepFilter filter, int pageIndex, int pageSize, out int totalCount);

        List<Sheep> GetAllSheep(SheepFilter filter, int pageIndex, int pageSize, out int totalCounts);

        Sheep GetSheepById(string id);

        /// <summary>
        /// 单只羊饲料出库专用
        /// </summary>
        /// <param name="shepfolds"></param>
        /// <returns></returns>
        List<Sheep4FeedOutWarehouse> GetSheep4FeedOutWarehouse(List<string> shepfolds);

        bool IsSheepExist(string id);

        /// <summary>
        /// 查询两只羊 
        /// </summary>
        /// <param name="maleId"></param>
        /// <param name="femaleId"></param>
        /// <returns></returns>
        List<Sheep> GetTwoSheeps(string maleId, string femaleId);

        /// <summary>
        /// 根据羊只编号集合获取圈舍集合
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<Sheep> GetSheepByIds(IEnumerable<string> ids);

        /// <summary>
        /// 验证是否存在相同名称的羊只品种
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        int GetBreedCountByName(string name);

        void AddBreed(string id, string name, string description, string operatorId, DateTime createTime, string remark);

        List<Breed> GetBreed(BreedFilter filter);
        List<Breed> GetBreed(BreedFilter filter, int rowsCount);

        Breed GetBreedByName(string name);

        List<Sheepfold> GetSheepfold(SheepfoldFilter filter);
        List<Sheepfold> GetSheepfold(SheepfoldFilter filter, int rowsCount);
        /// <summary>
        /// 获取系统默认的死亡、销售羊只的圈舍编号
        /// </summary>
        /// <returns></returns>
        string GetSysSheepfoldId(string administrator, string operatorId);

        void AddSheep(string id, string serialNumber, string breedId, GenderEnum gender, GrowthStageEnum growthStage, OriginEnum origin, float? birthWeight, int compatriotNumber, DateTime? birthDay, string fatherId, string motherId, string sheepfoldId, SheepStatusEnum sheepStatusEnum, string principalId, string operatorId, DateTime createTime, string remark);

        void AddSheep(string id, string serialNumber, string breedId, GenderEnum gender, GrowthStageEnum growthStage, OriginEnum origin, float? birthWeight, int compatriotNumber, DateTime? birthDay, string fatherId, string motherId, string sheepfoldId, SheepStatusEnum sheepStatusEnum, string principalId, string operatorId, DateTime createTime, string remark, string buySource, decimal buyMoney, float? buyWeight, DateTime buyOperationDate, string buyPrincipalId, string buyRemark);

        int GetCurMonthBirthCount();

        void UpdateSheep(string breedId, GenderEnum gender, GrowthStageEnum growthStage, OriginEnum origin, float? birthWeight, int compatriotNumber, DateTime? birthDay, float? ablactationWeight, DateTime? ablactationDate, string fatherId, string motherId, string sheepfoldId, SheepStatusEnum sheepStatusEnum, string principalId, string remark, string id);

        /// <summary>
        /// 查询当前编号的羊只数量
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        int GetSheepCountBySerialNum(string serialNumber);

        /// <summary>
        /// 获取指定名称的圈舍数量
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        int GetSheepFoldCountByName(string name);

        void AddSheepFold(string id, string name, string administrator, string operatorId, DateTime createTime, string remark);
        Sheepfold GetSheepFoldById(string id);
        void UpdateSheepFold(string name, string administrator, string remark, string id);


        Farm GetFarm();

        void AddFarm(string id, string name, string contacts, string phone, string address, string code, string businessScope, string qualifications, string remark);

        void UpdateFarm(string name, string contacts, string phone, string address, string code, string businessScope, string qualifications, string remark, string id);

        #endregion

        #region System

        /// <summary>
        /// 获取用户是否启用，错误次数，上次错误时间
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        List<Login> GetLoginMsg(string userName);

        string Login(string userName, string password);

        void UpdateLoginErrorTimes(string userName, int errorTimes, DateTime lastErrorTime);

        void UpdatePassword(string password, string id);

        int GetUserCount(string password, string id);

        List<SheepParameter> GetSheepParameters();

        void UpdateSheepParameters(List<UpdateSheepParameter> list, int deliveryRangeDays);

        List<FeedCritical> GetFeedCritical();
        List<MedicineCritical> GetMedicineCritical();

        void AddCritical(List<UpdateCritical> listAdd);
        void UpdateCritical(List<UpdateCritical> listUpdate);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pm">包含value与range</param>
        /// <returns></returns>
        List<PreDeliveryRemaindful> GetPreDeliveryRemaindful(SheepParameter pm);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pm">包含value与range</param>
        /// <returns></returns>
        List<PreAblactationRemaindful> GetPreAblactationRemaindful(int value);
        List<SheepParameter> GetSheepParameterValue(SettingsEnum settingsEnum);

        List<FeedRemaindful> GetFeedRemaindful();

        List<MedicineRemaindful> GetMedicineRemaindful();

        List<PreDeliveryRemaindful> GetPreDeliverySerialNumber(SheepParameter pm);

        List<PreAblactationRemaindful> GetPreAblactationSerialNumber(int value);

        /// <summary>
        /// 增加用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="errorTimes"></param>
        /// <param name="lastErrorTime"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="createTime"></param>
        /// <param name="isEnabled"></param>
        /// <param name="remark"></param>
        void AddUser(string id, string userName, string password, int errorTimes, DateTime lastErrorTime, string operatorId, DateTime createTime, bool isEnabled, string remark);

        /// <summary>
        /// 根据用户名获取用户数量
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        int GetUserCount(string userName);

        /// <summary>
        /// 增加角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="operatorId"></param>
        /// <param name="dateTime"></param>
        /// <param name="p"></param>
        /// <param name="remark"></param>
        void AddRole(string id, string name, string description, string operatorId, DateTime createTime, string remark);

        /// <summary>
        /// 增加权限
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="URL"></param>
        /// <param name="operatorId"></param>
        /// <param name="dateTime"></param>
        /// <param name="p"></param>
        /// <param name="remark"></param>
        void AddPermission(string id, string name, string description, string URL, string operatorId, DateTime createTime, string remark);

        Role GetRoleById(string id);

        Permission GetPermissionById(string id);

        void UpdateRole(string name, string description, string remark, string id);

        void UpdatePermission(string name, string description,  string remark, string id);

        List<Permission> GetAllPermissions(PermissionFilter filter, int pageIndex, int pageSize, out int totalCount);

        List<Role> GetAllRoles(RoleFilter filter, int pageIndex, int pageSize, out int totalCount);

        List<Permission> GetPermissionsByRoleId(string roleId);

        List<Permission> GetAllPermissionByUserId(string userId);

        List<Permission> GetPermissionByUserId(string userId);

        List<Role> GetAllRoleByUserId(string userId);

        List<Permission> GetRoleAvailablePermissions(string keyWord, string roleId);

        List<Permission> GetUserAvailablePermissions(string keyWord, string userId);

        List<Role> GetUserAvailableRoles(string keyWord, string userId);

        void DeletePermission(string id);

        void DeleteRole(string id);

        void GrantPermission2Role(List<string> permissionIds, string roleId);

        void GrantRole2User(List<string> roleIds, string userId);

        void GrantPermission2User(List<string> permissionIds, string userId);

        /// <summary>
        /// 删除指定角色的所有已权限
        /// </summary>
        /// <param name="roleId"></param>
        void DeleteRolePermission(string roleId);

        /// <summary>
        /// 删除指定用户的所有角色
        /// </summary>
        /// <param name="userId"></param>
        void DeleteUserRole(string userId);

        /// <summary>
        /// 删除指定用户的所有权限
        /// </summary>
        /// <param name="userId"></param>
        void DeleteUserPermission(string userId);

        /// <summary>
        /// 添加分场
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="manager"></param>
        void AddSplitYard(string id, string name, string manager);

        SplitYard GetSplitYardById(string id);

        List<SplitYard> GetAllSplitYard(SplitYardFilter filter, int pageIndex, int pageSize, out int totalCount);

        void UpdateSplitYard(string name, string manager, string id);

        #endregion

        #region Multiplying

        /// <summary>
        /// 交配记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="femaleId"></param>
        /// <param name="maleId"></param>
        /// <param name="matingDate"></param>
        /// <param name="isRemindful">是否体型</param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="dateTime"></param>
        /// <param name="remark"></param>
        void AddMating(string id, string femaleId, string maleId, DateTime matingDate, bool isRemindful, string principalId, string operatorId, DateTime dateTime, string remark);

        void UpdateMatingDisRemindful(string maleId);

        List<Mating> GetMating(MatingFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<Mating> GetMating(MatingFilter filter, int rowsCount);

        Mating GetMatingById(string id);

        void DeleteMating(string id);

        List<MatingCount> GetMatingCount(string sheepId, int? count, DateTime? startDate, DateTime? endDate, int pageIndex, int pageSize, out int totalCount);
        List<MatingCount> GetMatingCount(string sheepId, int? count, DateTime? startDate, DateTime? endDate, int rowsCount);

        Mating GetLatestMatingByFemaleId(string sheepId);

        object GetLatestAbortionDateBySheepId(string sheepId);

        object GetlatestDeliveryDateBySheepId(string sheepId);

        /// <summary>
        /// 流产记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sheepId"></param>
        /// <param name="reason"></param>
        /// <param name="dispose"></param>
        /// <param name="abortionDate"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="dateTime"></param>
        /// <param name="remark"></param>
        void AddAbortion(string id, string sheepId, string reason, string dispose, DateTime abortionDate, string principalId, string operatorId, DateTime createTime, string remark);

        /// <summary>
        /// 添加分娩信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="femaleId"></param>
        /// <param name="deliveryWay"></param>
        /// <param name="lambCount"></param>
        /// <param name="liveCount"></param>
        /// <param name="deliveryDate"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="dateTime"></param>
        /// <param name="remark"></param>
        void AddDelivery(string id, string femaleId, DeliveryWayEnum deliveryWay, MidwiferyReasonEnum deliverReason, string deliverReasonOtherDetail, int? liveMaleCount, int? liveFemaleCount, int totalCount, DateTime deliveryDate, string principalId, string operatorId, DateTime createTime, string remark);
        /// <summary>
        /// 添加分娩信息（附带羔羊）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="femaleId"></param>
        /// <param name="deliveryWay"></param>
        /// <param name="DeliverReason"></param>
        /// <param name="DeliverReasonOtherDetail"></param>
        /// <param name="LambMaleCount"></param>
        /// <param name="LambFemaleCount"></param>
        /// <param name="liveCount"></param>
        /// <param name="deliveryDate"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="createTime"></param>
        /// <param name="remark"></param>
        /// <param name="lambList">羔羊信息列表</param>
        /// <param name="breedId">品种Id</param>
        /// <param name="fatherId">羔羊父编号</param>
        void AddDelivery(string id, string femaleId, DeliveryWayEnum deliveryWay, MidwiferyReasonEnum deliverReason, string deliverReasonOtherDetail, int? liveMaleCount, int? liveFemaleCount, int totalCount, DateTime deliveryDate, string principalId, string operatorId, DateTime createTime, string remark, List<Chanyi.Shepherd.QueryModel.AddModel.BaseInfo.Sheep> lambList, string lambBreedId, string fatherId);
        /// <summary>
        /// 获取交配过的公羊、母羊品种性别
        /// </summary>
        /// <param name="femaleId"></param>
        /// <returns></returns>
        List<Sheep> GetMatingBreedInfoByFemaleId(string femaleId);

        /// <summary>
        /// 断奶管理
        /// </summary>
        /// <param name="sheepId"></param>
        /// <param name="ablactationWeight"></param>
        /// <param name="ablactationDate"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="dateTime"></param>
        /// <param name="remark"></param>
        void AddAblactation(string id, string sheepId, float ablactationWeight, DateTime ablactationDate, string principalId, string operatorId, DateTime createTime, string remark);

        /// <summary>
        /// 根据SheepId查看断奶羊只记录
        /// </summary>
        /// <param name="sheepId"></param>
        /// <returns></returns>
        int GetAblactation(string sheepId);

        List<Delivery> GetDelivery(DeliveryFilter filter, int pageIndex, int pageSize, out int totalCount);

        List<Delivery> GetDelivery(DeliveryFilter filter, int rowsCount);

        Delivery GetDeliveryById(string id);

        void DeleteDelivery(string id);

        List<Abortion> GetAboration(AbortionFilter filter, int pageIndex, int pageSize, out int totalCount);

        List<Abortion> GetAboration(AbortionFilter filter, int rowsCount);
        Abortion GetAbortionById(string id);

        void DeleteAbortion(string id);

        List<Ablactation> GetAblactation(AblactationFilter filter, int pageIndex, int pageSize, out int totalCount);

        List<Ablactation> GetAblactation(AblactationFilter filter, int rowsCount);
        Ablactation GetAblactationById(string id);


        void UpdateMating(string femaleId, string maleId, DateTime matingDate, bool isRemindful, string principalId, string remark, string id);

        void UpdateAblactation(float ablactationWeight, DateTime ablactationDate, string sheepId, string principalId, string remark, string id);

        void UpdateAbortion(string reason, string dispose, DateTime abortionDate, string principalId, string remark, string id);

        void UpdateDelivery(DeliveryWayEnum deliveryWay, MidwiferyReasonEnum deliverReason, string deliverReasonOtherDetail, int? liveMaleCount, int? liveFemaleCount, int totalCount, DateTime deliveryDate, string principalId, string remark, string id);

        #endregion

        #region Breeding

        /// <summary>
        /// 添加种羊鉴定信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="studSheepId"></param>
        /// <param name="matingAbility"></param>
        /// <param name="weight"></param>
        /// <param name="habitusScore"></param>
        /// <param name="assessDate"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="dateTime"></param>
        /// <param name="remark"></param>
        void AddAssessStudsheep(string id, string studSheepId, float matingAbility, float weight, float habitusScore, DateTime assessDate, string principalId, string operatorId, DateTime createTime, string remark);

        /// <summary>
        /// 根据羊编号获取种羊鉴定过的数量
        /// </summary>
        /// <param name="studSheepId"></param>
        /// <returns></returns>
        int GetStudsheepCount(string studSheepId);

        /// <summary>
        /// 根据羊编号获取第一次鉴定过的数量
        /// </summary>
        /// <param name="sheepId"></param>
        /// <returns></returns>
        int GetFirstAssessCount(string sheepId);

        /// <summary>
        /// 添加第一次鉴定的信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sheepId"></param>
        /// <param name="weight"></param>
        /// <param name="habitusScore"></param>
        /// <param name="assessDate"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="dateTime"></param>
        /// <param name="remark"></param>
        void AddFirstAssess(string id, string sheepId, float weight, float habitusScore, DateTime assessDate, string principalId, string operatorId, DateTime createTime, string remark);

        /// <summary>
        /// 根据羊编号获取第二次鉴定过的数量
        /// </summary>
        /// <param name="sheepId"></param>
        /// <returns></returns>
        int GetSecondAssess(string sheepId);

        /// <summary>
        /// 添加第二次鉴定的信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sheepId"></param>
        /// <param name="BreedFeatureScore"></param>
        /// <param name="GenitaliaScore"></param>
        /// <param name="weight"></param>
        /// <param name="habitusScore"></param>
        /// <param name="assessDate"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="dateTime"></param>
        /// <param name="remark"></param>
        void AddSecondAssess(string id, string sheepId, float breedFeatureScore, float genitaliaScore, float weight, float habitusScore, DateTime assessDate, string principalId, string operatorId, DateTime createTime, string remark);

        /// <summary>
        /// 根据羊编号获取第三次鉴定过的数量
        /// </summary>
        /// <param name="sheepId"></param>
        /// <returns></returns>
        int GetThirdAssess(string sheepId);

        /// <summary>
        /// 添加第三次鉴定的信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sheepId"></param>
        /// <param name="MatingAbility"></param>
        /// <param name="weight"></param>
        /// <param name="habitusScore"></param>
        /// <param name="assessDate"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="dateTime"></param>
        /// <param name="remark"></param>
        void AddThirdAssess(string id, string sheepId, float matingAbility, float weight, float habitusScore, DateTime assessDate, string principalId, string operatorId, DateTime createTime, string remark);

        void AddExceptAssessSheep(string id, string sheepId, string reason, GrowthStageEnum growthStageEnum, string principalId, string operatorId, DateTime createTime, string remark);

        void DeleteExceptAssessSheep(string id);

        List<AssessStudsheep> GetAssessStudsheep(AssessStudsheepFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<AssessStudsheep> GetAssessStudsheep(AssessStudsheepFilter filter, int rowCount);
        AssessStudsheep GetAssessStudsheepById(string id);

        List<FirstAssess> GetFirstAssess(FirstAssessFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<FirstAssess> GetFirstAssess(FirstAssessFilter filter, int rowCount);
        FirstAssess GetFirstAssessById(string id);

        List<SecondAssess> GetSecondAssess(SecondAssessFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<SecondAssess> GetSecondAssess(SecondAssessFilter filter, int rowCount);
        SecondAssess GetSecondAssessById(string id);

        List<ThirdAssess> GetThirdAssess(ThirdAssessFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<ThirdAssess> GetThirdAssess(ThirdAssessFilter filter, int rowCount);
        ThirdAssess GetThirdAssessById(string id);

        List<ExceptAssess> GetExceptAssess(ExceptAssessFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<ExceptAssess> GetExceptAssess(ExceptAssessFilter filter, int rowCount);

        void UpdateAssessStudsheep(float matingAbility, float weight, float habitusScore, DateTime assessDate, string principalId, string remark, string id);

        void UpdateFirstAssess(float weight, float habitusScore, DateTime assessDate, string principalId, string remark, string id);

        void UpdateSecondAssess(float breedFeatureScore, float genitaliaScore, float weight, float habitusScore, DateTime assessDate, string principalId, string remark, string id);

        void UpdateThirdAssess(float matingAbility, float weight, float habitusScore, DateTime assessDate, string principalId, string remark, string id);

        #endregion

        #region HR

        int GetEmployeeCountByName(string name);

        int GetEmployeeCountBySerialNum(string serNum);

        void AddEmployee(string id, string name, GenderEnum gender, string idNum, DateTime entryDate, decimal salary, string serialNum, string dutyId, EmployeeStatusEnum status, string principalId, string operatorId, DateTime createTime, string remark);

        List<Employee> GetEmployee(EmployeeFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<Employee> GetEmployee(EmployeeFilter filter, int rowsCount);
        Employee GetEmployeeById(string id);
        Employee GetEmployeeByName(string name);
        Employee GetEmployeeBySerialNum(string serNum);

        void UpdateEmployee(string name, GenderEnum gender, string idNum, DateTime entryDate, decimal salary, string serialNum, string dutyId, EmployeeStatusEnum status, string principalId, string remark, string id);

        void AddQuit(string id, string employeeId, string reason, DateTime quitDate, string principalId, string operatorId, DateTime createTime, string remark);
        List<Quit> GetQuit(QuitFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<Quit> GetQuit(QuitFilter filter, int rowsCount);

        int GetDutyCountByName(string name);

        void AddDuty(string id, string name, string description, string operatorId, DateTime createTime, string remark);

        List<User> GetUser(int pageIndex, int pageSize, out int totalCount);
        List<User> GetUser(int rowsCount);
        User GetUserById(string id);

        void UpdateUser(bool isEnabled, string remark, string id);

        List<Cooperater> GetCooperater(CooperaterFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<Cooperater> GetCooperater(CooperaterFilter filter, int rowsCount);
        Cooperater GetCooperaterById(string id);

        void UpdatePurchaser(string name, string department, string contactInfo, string principalId, string remark, string id);

        void AddPurchaser(string id, string name, string department, string contactInfo, string principalId, string operatorId, DateTime createTime, string remark);

        #endregion

        #region GroupManage

        /// <summary>
        /// 转圈
        /// </summary>
        /// <param name="sheepIds"></param>
        /// <param name="targetSheepfold"></param>
        void AddMoveSheepfold(List<Sheep> sheeps, string targetSheepfold, string principalId, string operatorId, DateTime createTime, string remark);

        void AddDeathManage(string id, string sheepId, string reason, DeathDisposeEnum dispose, DateTime deathDate, string principalId, string operatorId, DateTime createTime, string remark, string sysSheepfoldId);

        List<DeathManage> GetDeathManage(DeathManageFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<DeathManage> GetDeathManage(DeathManageFilter filter, int rowsCount);
        DeathManage GetDeathManageById(string id);
        void DeleteDeathManage(string id);

        void UpdateDeathManage(string reason, DeathDisposeEnum dispose, DateTime deathDate, string principalId, string remark, string id);

        List<MoveSheepfold> GetMoveSheepfold(MoveSheepfoldFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<MoveSheepfold> GetMoveSheepfold(MoveSheepfoldFilter filter, int rowsCount);

        #endregion

        #region Assist

        //List<DiseaseType> GetDiseaseType(string pid);

        //List<Disease> GetDiseaseByType(string typeId);

        ///// <summary>
        ///// 症状名称模糊搜索疾病
        ///// </summary>
        ///// <param name="symptomName">症状名称</param>
        ///// <returns></returns>
        //List<Disease> GetDiseaseBySymptomName(string symptomName);


        ///// <summary>
        ///// 交叉症状ID搜索疾病（多种症状交叉）
        ///// </summary>
        ///// <param name="symptomIds"></param>
        ///// <returns></returns>
        //List<Disease> GetCrossDiseaseBySymptomIds(string[] symptomIds);

        #endregion

        #region Formula

        List<Formula> GetFormula(FormulaFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<Formula> GetFormula(FormulaFilter filter, int rowsCount);
        List<FormulaFeed> GetFormulaFeedById(string formulaId);


        List<SimpleFeed> GetSimpleFeed();

        List<FormulaNutrient> GetFormulaNutrient(FormulaNutrientFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<FormulaNutrient> GetFormulaNutrient(FormulaNutrientFilter filter, int rowsCount);
        FormulaNutrient GetFormulaNutrientById(string id);

        void UpdateFormulaStatus(bool isEnable, string id);

        void AddFormulaNutrient(string id, string name, float? dailyGain, float? CP, float? DMI, float? EE, float? CF, float? NFE, float? Ash, float? NDF, float? ADF, float? Starch, float? Ga, float? AllP, float? Arg, float? His, float? Ile, float? Leu, float? Lys, float? Met, float? Cys, float? Phe, float? Tyr, float? Thr, float? Trp, float? Val, float? P, float? Na, float? Cl, float? Mg, float? K, float? Fe, float? Cu, float? Mn, float? Zn, float? Se, float? Carotene, float? VE, float? VB1, float? VB2, float? PantothenicAcid, float? Niacin, float? Biotin, float? Folic, float? Choline, float? VB6, float? VB12, float? LinoleicAcid, float? Salt, bool isEditable, string principalId, string operatorId, DateTime createTime, string remark);

        void UpdateFormulaNutrient(string name, float? dailyGain, float? CP, float? DMI, float? EE, float? CF, float? NFE, float? Ash, float? NDF, float? ADF, float? Starch, float? Ga, float? AllP, float? Arg, float? His, float? Ile, float? Leu, float? Lys, float? Met, float? Cys, float? Phe, float? Tyr, float? Thr, float? Trp, float? Val, float? P, float? Na, float? Cl, float? Mg, float? K, float? Fe, float? Cu, float? Mn, float? Zn, float? Se, float? Carotene, float? VE, float? VB1, float? VB2, float? PantothenicAcid, float? Niacin, float? Biotin, float? Folic, float? Choline, float? VB6, float? VB12, float? LinoleicAcid, float? Salt, string principalId, string remark, string id);

        /// <summary>
        /// 当前营养标准是否可编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool IsFormulaNutrientEditable(string id);

        void AddFormula(string id, Dictionary<string, float> formulaFeed, string name, string applyTo, string sideEffect, bool isDel, string principalId, string operatorId, DateTime createTime, string remark);

        List<Formula> GetFormulabyName(string name);

        #endregion

        #region Inputs

        #region 饲料
        List<Feed> GetFeedKindId(string nameId, string typeId, string areaId);

        /// <summary>
        /// 获取指定饲料库存
        /// </summary>
        /// <param name="kindId"></param>
        /// <returns></returns>
        object GetFeedInventoryAmount(string kindId);

        /// <summary>
        /// 获取指定操作日期之后的出入库数量
        /// </summary>
        /// <param name="kindId"></param>
        /// <param name="operationDate"></param>
        /// <returns></returns>
        List<InOutAmount> GetFeedInOutAmount(string kindId, DateTime operationDate);

        /// <summary>
        /// 饲料出入库
        /// </summary>
        /// <param name="id"></param>
        /// <param name="kindId"></param>
        /// <param name="amount"></param>
        /// <param name="operationDate"></param>
        /// <param name="direction"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="createTime"></param>
        /// <param name="remark"></param>
        void AddFeedInOutWarehouse(string id, string kindId, float amount, DateTime operationDate, InOutWarehouseDirectionEnum direction, OutWarehouseDispositonEnum dispositon, string principalId, string operatorId, DateTime createTime, string remark, bool hasInventory);

        /// <summary>
        /// 批量饲料出库
        /// </summary>
        /// <param name="listSheeps">羊只Id、SheepfoldId、GrowthStage</param>
        /// <param name="listPrices">FeedId、Price</param>
        /// <param name="operationDate"></param>
        /// <param name="inOutWarehouseDirectionEnum"></param>
        /// <param name="outWarehouseDispositonEnum"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="dateTime"></param>
        /// <param name="remark"></param>
        void AddFeedBatchOutWarehouse(List<Sheep4FeedOutWarehouse> listSheeps, List<FeedPrice> listPrices, Dictionary<string, float> dictFeedAmount, DateTime operationDate, InOutWarehouseDirectionEnum inOutWarehouseDirectionEnum, OutWarehouseDispositonEnum outWarehouseDispositonEnum, string principalId, string operatorId, DateTime createTime, string remark);

        void AddAreaName(string id, string name, string operatorId, DateTime createTime, string remark);
        int GetAreaNameCount(string name);

        void AddInputName(string id, string name, string category, string operatorId, DateTime createTime, string remark);
        int GetInputNameCount(string name, string category);

        void AddFeed(string id, string feedNameId, string typeNameId, string areaId, string description, float? CP, float? DMI, float? EE, float? CF, float? NFE, float? Ash, float? NDF, float? ADF, float? Starch, float? Ga, float? Arg, float? His, float? Ile, float? Leu, float? Lys, float? Met, float? Cys, float? Phe, float? Tyr, float? Thr, float? Trp, float? Val, float? P, float? Na, float? Cl, float? Mg, float? K, float? Fe, float? Cu, float? Mn, float? Zn, float? Se, float? Carotene, float? VE, float? VB1, float? VB2, float? PantothenicAcid, float? Niacin, float? Biotin, float? Folic, float? Choline, float? VB6, float? VB12, float? LinoleicAcid, float? AllP, string operatorId, DateTime createTime);

        List<Feed> GetFeed(FeedFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<Feed> GetFeed(FeedFilter filter, int rowCount);

        Feed GetFeedByKindId(string kindId);

        List<FeedDetail> GetFeedDetail(string kindId);

        List<FeedInOut> GetFeedInOut(FeedInOutFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<FeedInOut> GetFeedInOut(FeedInOutFilter filter, int rowCount);

        FeedInOut GetFeedInOutDetailById(string id);

        List<FeedInventory> GetFeedInventory(FeedInventoryFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<FeedInventory> GetFeedInventory(FeedInventoryFilter filter, int rowsCount);
        List<FeedInventory> GetFeedInventory();

        bool ValidateFeed(string id, string feedNameId, string typeNameId, string areaId);

        void UpdateFeed(string feedNameId, string typeNameId, string areaId, string description, float? CP, float? DMI, float? EE, float? CF, float? NFE, float? Ash, float? NDF, float? ADF, float? Starch, float? Ga, float? Arg, float? His, float? Ile, float? Leu, float? Lys, float? Met, float? Cys, float? Phe, float? Tyr, float? Thr, float? Trp, float? Val, float? P, float? Na, float? Cl, float? Mg, float? K, float? Fe, float? Cu, float? Mn, float? Zn, float? Se, float? Carotene, float? VE, float? VB1, float? VB2, float? PantothenicAcid, float? Niacin, float? Biotin, float? Folic, float? Choline, float? VB6, float? VB12, float? LinoleicAcid, float? AllP, string id);

        bool IsFeedEditable(string id);

        List<FeedWithAllFileds> GetFeedWithAllFileds();

        #endregion

        #region 药品
        List<Medicine> GetMedicineId(string nameId, string manufacturerId, string typeId);
        List<Medicine> GetMedicineKindId(string nameId, string manufacturerId, string typeId, DateTime expirationDate);
        object ValidateMedicineCount(string kindId);

        /// <summary>
        /// 获取指定操作日期之后的出入库数量
        /// </summary>
        /// <param name="kindId"></param>
        /// <param name="operationDate"></param>
        /// <returns></returns>
        List<InOutAmount> GetMedicineInOutAmount(string kindId, DateTime operationDate);

        void AddManufacturer(string id, string name, string department, string contactInfo, string principalId, string operatorId, DateTime createTime, string remark);
        void AddMedicine(string id, string nameId, string manufacturerId, string typeId, string medicineUnit, string operatorId, DateTime createTime, string remark);
        /// <summary>
        /// 已经存在该过期时间的药品，直接添加
        /// </summary>
        /// <param name="id"></param>
        /// <param name="kindId">关键性表的Id（T_MedicineCrucial）</param>
        /// <param name="amount"></param>
        /// <param name="operationDate"></param>
        /// <param name="direction"></param>
        /// <param name="dispositon"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="createTime"></param>
        /// <param name="remark"></param>
        /// <param name="hasInvertory"></param>
        void AddMedicineInOutWarehouse(string id, string kindId, float amount, DateTime operationDate, InOutWarehouseDirectionEnum direction, OutWarehouseDispositonEnum dispositon, string principalId, string operatorId, DateTime createTime, string remark, bool hasInvertory);
        /// <summary>
        /// 入库、顺便添加药品过期时间（已经存在该过期时间的药品）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="kindId">药品表的Id</param>
        /// <param name="expirationDate">过期时间</param>
        /// <param name="amount"></param>
        /// <param name="operationDate"></param>
        /// <param name="direction"></param>
        /// <param name="dispositon"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="createTime"></param>
        /// <param name="remark"></param>
        /// <param name="hasInvertory"></param>
        void AddMedicineInOutWarehouse(string id, string kindId, DateTime expirationDate, float amount, DateTime operationDate, InOutWarehouseDirectionEnum direction, OutWarehouseDispositonEnum dispositon, string principalId, string operatorId, DateTime createTime, string remark);

        List<Medicine> GetMedicine(MedicineFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<Medicine> GetMedicine(MedicineFilter filter, int rowsCount);
        Medicine GetMedicineByKindId(string kindId);

        List<MedicineInventory> GetMedicineInventory(MedicineInventoryFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<MedicineInOut> GetMenicineInOut(MedicineInOutFilter filter, int rowsCount);

        List<MedicineInOut> GetMenicineInOut(MedicineInOutFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<MedicineInventory> GetMedicineInventory(MedicineInventoryFilter filter, int rowsCount);
        MedicineInOut GetMenicineInOutDetailById(string id);

        bool ValidateMedicine(string id, string nameId, string manufacturerId, string typeId);

        void UpdateMedicine(string nameId, string manufacturerId, string typeId, string remark, string id);
        #endregion

        #region 其他

        List<Other> GetOtherKindId(string nameId);

        void AddOther(string id, string name, string unit, string operatorId, DateTime createTime, string remark);

        object ValidateOtherCount(string kindId);
        /// <summary>
        /// 获取指定操作日期之后的出入库数量
        /// </summary>
        /// <param name="kindId"></param>
        /// <param name="operationDate"></param>
        /// <returns></returns>
        List<InOutAmount> GetOtherInOutAmount(string kindId, DateTime operationDate);

        void AddOtherInOutWarehouse(string id, string kindId, float amount, DateTime operationDate, InOutWarehouseDirectionEnum direction, OutWarehouseDispositonEnum dispositon, string principalId, string operatorId, DateTime createTime, string remark, bool hasInvertory);

        bool ValidateOther(string id, string name);

        List<Other> GetOther(OtherFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<Other> GetOther(OtherFilter filter, int rowCount);

        Other GetOtherByKindId(string kindId);

        List<OtherInOut> GetOtherInOut(OtherInOutFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<OtherInOut> GetOtherInOut(OtherInOutFilter filter, int rowCount);
        OtherInOut GetOtherInOutDetailById(string id);

        List<OtherInventory> GetOtherInventory(OtherInventoryFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<OtherInventory> GetOtherInventory(OtherInventoryFilter filter, int rowCount);

        void UpdateOther(string name, string unit, string remark, string id);

        #endregion

        #endregion

        #region Finance

        #region 卖
        void AddSellSheep(List<AddSellSheep> list, decimal totalPrice, float totalWeight, string purchaserId, DateTime operationDate, string principalId, string operatorId, DateTime createTime, string remark, string sysSheepfoldId);
        List<SellSheep> GetSellSheepIds(List<string> sheepIds);

        List<SellSheepBatch> GetSellSheepBath(SellSheepBatchFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<SellSheepBatch> GetSellSheepBath(SellSheepBatchFilter filter, int rowsCount);

        List<SellSheep> GetSellSheep(SellSheepFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<SellSheep> GetSellSheep(SellSheepFilter filter, int rowsCount);

        List<SellSheep> GetSellSheep(string batchId, int pageIndex, int pageSize, out int totalCount);

        void AddSellManure(string id, decimal price, string purchaserId, DateTime operationDate, string principalId, string operatorId, DateTime createTime, string remark);

        List<SellManure> GetSellManure(SellManureFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<SellManure> GetSellManure(SellManureFilter filter, int rowsCount);

        void AddSellWool(string id, float amount, decimal price, string purchaserId, DateTime operationDate, string principalId, string operatorId, DateTime createTime, string remark);

        List<SellWool> GetSellWool(SellWoolFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<SellWool> GetSellWool(SellWoolFilter filter, int rowsCount);
        /// <summary>
        /// 根据LinkIds查询所有的SellFeed的集合
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<SellFeed> GetSellFeedIds(List<string> ids);
        void AddSellFeed(List<AddSellInput> list, string operatorId, DateTime createTime, string remark);
        List<SellFeed> GetSellFeed(SellFeedFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<SellFeed> GetSellFeed(SellFeedFilter filter, int rowsCount);

        /// <summary>
        /// 根据LinkIds查询所有的SellOther的集合
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<SellOther> GetSellOtherIds(List<string> ids);
        void AddSellOther(List<AddSellInput> list, string operatorId, DateTime createTime, string remark);
        List<SellOther> GetSellOther(SellOtherFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<SellOther> GetSellOther(SellOtherFilter filter, int rowsCount);

        #endregion

        #region 买

        void AddElectricCharge(string id, float amount, decimal money, DateTime operationDate, string principalId, string operatorId, DateTime createTime, string remark);

        List<ElectricCharge> GetElectricCharge(ElectricChargeFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<ElectricCharge> GetElectricCharge(ElectricChargeFilter filter, int rowsCount);

        void AddWaterRate(string id, float amount, decimal money, DateTime operationDate, string principalId, string operatorId, DateTime createTime, string remark);

        List<WaterRate> GetWaterRate(WaterRateFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<WaterRate> GetWaterRate(WaterRateFilter filter, int rowsCount);

        void AddPayoff(string id, string employeeId, decimal money, DateTime operationDate, string principalId, string operatorId, DateTime createTime, string remark);

        List<Payoff> GetPayoff(PayoffFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<Payoff> GetPayoff(PayoffFilter filter, int rowsCount);

        void AddIncidentals(string id, decimal money, DateTime operationDate, string principalId, string operatorId, DateTime createTime, string remark);

        List<Incidentals> GetIncidentals(IncidentalsFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<Incidentals> GetIncidentals(IncidentalsFilter filter, int rowsCount);

        void AddBuySheep(string id, string sheepId, string source, decimal money, DateTime operationDate, string principalId, string operatorId, DateTime createTime, string remark, float? weight);

        List<BuySheep> GetBuySheep(BuySheepFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<BuySheep> GetBuySheep(BuySheepFilter filter, int rowsCount);

        void AddBuyFeed(Dictionary<string, decimal> inputExpenditure, DateTime operationDate, string principalId, string operatorId, DateTime createTime, string remark);

        List<BuyFeed> GetBuyFeedIds(List<string> linkIds);

        List<BuyFeed> GetBuyFeed(BuyFeedFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<BuyFeed> GetBuyFeed(BuyFeedFilter filter, int rowsCount);

        void AddBuyMedicine(Dictionary<string, decimal> inputExpenditure, DateTime operationDate, string principalId, string operatorId, DateTime createTime, string remark);

        List<BuyMedicine> GetBuyMedicineIds(List<string> linkIds);

        List<BuyMedicine> GetBuyMedicine(BuyMedicineFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<BuyMedicine> GetBuyMedicine(BuyMedicineFilter filter, int rowsCount);

        void AddBuyOther(Dictionary<string, decimal> inputExpenditure, DateTime operationDate, string principalId, string operatorId, DateTime createTime, string remark);

        List<BuyOther> GetBuyOtherIds(List<string> linkIds);

        List<BuyOther> GetBuyOther(BuyOtherFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<BuyOther> GetBuyOther(BuyOtherFilter filter, int rowsCount);

        /// <summary>
        /// 指定饲料当前的价格
        /// </summary>
        /// <param name="listFeedIds">饲料KindId集合</param>
        /// <returns></returns>
        List<FeedPrice> GetCurrentFeedPrices(IEnumerable<string> listFeedIds);

        #endregion

        #endregion

        #region DecisionAids
        /// <summary>
        /// 系谱
        /// </summary>
        /// <param name="sheepId"></param>
        /// <param name="depth">深度（上几代）</param>
        /// <returns></returns>
        List<FamilyTree> GetFamilyTree(string sheepId, int depth);
        /// <summary>
        /// 辅助配种
        /// </summary>
        /// <param name="sheepId"></param>
        /// <param name="gender">异性的性别</param>
        /// <returns></returns>
        List<AssistMating> GetAssistMating(string sheepId, int depth);

        //int VarifyTwoSheepsCanMating(string maleId, string femaleId);

        #endregion

        #region DiseaseControl

        //防疫

        /// <summary>
        /// 添加防疫信息
        /// </summary>
        /// <param name="name">防疫名称</param>
        /// <param name="vaccine">疫苗</param>
        /// <param name="executeDate">执行日期</param>
        /// <param name="effect">防疫效果</param>
        /// <param name="sheepFlock">羊群</param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        void AddAntiepidemic(string id, string name, string vaccine, DateTime executeDate, string effect, string sheepFlock, string principalId, string operatorId, DateTime createTime, string remark);

        List<Antiepidemic> GetAntiepidemic(AntiepidemicFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<Antiepidemic> GetAntiepidemic(AntiepidemicFilter filter, int rowsCount);
        Antiepidemic GetAntiepidemicById(string id);
        /// <summary>
        /// 编辑防疫信息
        /// </summary>
        /// <param name="name">防疫名称</param>
        /// <param name="vaccine">疫苗</param>
        /// <param name="executeDate">执行日期</param>
        /// <param name="effect">防疫效果</param>
        /// <param name="sheepFlock">羊群</param>
        /// <param name="principalId"></param>
        /// <param name="remark"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        void UpdateAntiepidemic(string name, string vaccine, DateTime executeDate, string effect, string sheepFlock, string principalId, string remark, string id);

        //防疫计划

        /// <summary>
        /// 添加防疫计划信息
        /// </summary>
        /// <param name="name">防疫名称</param>
        /// <param name="vaccine">疫苗</param>
        /// <param name="planExecuteDate">计划执行日期</param>
        /// <param name="effect">防疫效果</param>
        /// <param name="sheepFlock">羊群</param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        void AddAntiepidemicPlan(string id, string name, string vaccine, DateTime planExecuteDate, string sheepFlock, string principalId, string operatorId, DateTime createTime, string remark);
        List<AntiepidemicPlan> GetAntiepidemicPlan(AntiepidemicPlanFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<AntiepidemicPlan> GetAntiepidemicPlan(AntiepidemicPlanFilter filter, int rowsCount);
        AntiepidemicPlan GetAntiepidemicPlanById(string id);
        /// <summary>
        /// 编辑防疫计划信息
        /// </summary>
        /// <param name="name">防疫名称</param>
        /// <param name="vaccine">疫苗</param>
        /// <param name="executeDate">计划执行日期</param>
        /// <param name="effect">防疫效果</param>
        /// <param name="sheepFlock">羊群</param>
        /// <param name="principalId"></param>
        /// <param name="remark"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        void UpdateAntiepidemicPlan(string name, string vaccine, DateTime planExecuteDate, string sheepFlock, string principalId, string remark, string id);
        /// <summary>
        /// 执行防疫计划
        /// </summary>
        /// <param name="id"></param>
        /// <param name="planId"></param>
        /// <param name="name"></param>
        /// <param name="vaccine"></param>
        /// <param name="executeDate"></param>
        /// <param name="effect"></param>
        /// <param name="sheepFlock"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="createTime"></param>
        /// <param name="remark"></param>
        void ExecuteAntiepidemicPlan(string id, string planId, string name, string vaccine, DateTime executeDate, string effect, string sheepFlock, string principalId, string operatorId, DateTime createTime, string remark);

        //疾病治疗

        /// <summary>
        /// 疾病治疗
        /// </summary>
        /// <param name="sheepId">羊只Id</param>
        /// <param name="symptom">症状</param>
        /// <param name="startDate">疾病开始日期</param>
        /// <param name="disease">诊断（疾病）</param>
        /// <param name="treatmentPlan">治疗计划</param>
        /// <param name="treatmentDays">治疗时长</param>
        /// <param name="effect">治疗结果</param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        void AddTreatment(string id, string sheepId, string symptom, DateTime startDate, string disease, string treatmentPlan, int treatmentDays, string effect, string principalId, string operatorId, DateTime createTime, string remark);
        List<Treatment> GetTreatment(TreatmentFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<Treatment> GetTreatment(TreatmentFilter filter, int rowsCount);
        Treatment GetTreatmentById(string id);
        /// <summary>
        /// 更新疾病治疗
        /// </summary>
        /// <param name="symptom">症状</param>
        /// <param name="startDate">疾病开始日期</param>
        /// <param name="disease">诊断（疾病）</param>
        /// <param name="treatmentPlan">治疗计划</param>
        /// <param name="treatmentDays">治疗时长</param>
        /// <param name="effect">治疗结果</param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        void UpdateTreatment(string symptom, DateTime startDate, string disease, string treatmentPlan, int treatmentDays, string effect, string principalId, string remark, string id);


        #endregion

        #region 报表

        List<MultiplyReport> GetMultiplyReport(DateTime startDate, DateTime endDate);

        List<SellReport> GetSellReport(DateTime startDate, DateTime endDate);

        List<BuyReport> GetBuyReport(DateTime startDate, DateTime endDate);

        List<FinanceReport> GetFinanceReport(DateTime startDate, DateTime endDate);

        List<FeedReport> GetFeedInventoryReport(DateTime startDate, DateTime endDate);

        List<FeedSheepReport> GetFeedSheepReport(string sheepId);

        #endregion
    }
}

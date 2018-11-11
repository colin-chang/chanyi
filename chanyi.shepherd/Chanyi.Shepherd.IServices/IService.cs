using System;
using System.Collections.Generic;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.BaseInfo;
using Chanyi.Shepherd.QueryModel.Filter.HR;
using Chanyi.Shepherd.QueryModel.Model.BaseInfo;
using Chanyi.Shepherd.QueryModel.Model.System;
using Chanyi.Shepherd.QueryModel.Model.Multiplying;
using Chanyi.Shepherd.QueryModel.Filter.Multiplying;
using Chanyi.Shepherd.QueryModel.BindingFilter;
using Chanyi.Shepherd.QueryModel.Model.HR;
using Chanyi.Shepherd.QueryModel.Model.Assist;
using Chanyi.Shepherd.QueryModel.Model.Breeding;
using Chanyi.Shepherd.QueryModel.Filter.Breeding;
using Chanyi.Shepherd.QueryModel.Model.Group;
using Chanyi.Shepherd.QueryModel.Filter.Group;
using Chanyi.Shepherd.QueryModel.Model.Chart;
using Chanyi.Shepherd.QueryModel.Model.Formula;
using Chanyi.Shepherd.QueryModel.Filter.Formula;
using Chanyi.Shepherd.QueryModel.Model.Input;
using Chanyi.Shepherd.QueryModel.Filter.Input;
using Chanyi.Shepherd.QueryModel.Model.Finance;
using Chanyi.Shepherd.QueryModel.Filter.Finance;
using Chanyi.Shepherd.QueryModel.UpdateModel.System;
using Chanyi.Shepherd.QueryModel.Model.DiseaseControl;
using Chanyi.Shepherd.QueryModel.Filter.DiseaseControl;
using Chanyi.Shepherd.QueryModel.AddModel.Finance;
using Chanyi.Shepherd.QueryModel.Model.ReportForms;
using Chanyi.Shepherd.QueryModel.Filter.System;

namespace Chanyi.Shepherd.IServices
{
    public interface IService
    {
        #region Bind

        ServiceResult<bool> Initialize();

        List<SheepBind> GetSheepBind(SheepBindFilter filter);

        /// <summary>
        /// 获取羊只父母编号绑定
        /// 包含自己的父母可能已经被一键排除为育肥羊
        /// </summary>
        /// <param name="childId">当前羊只Id</param>
        /// <returns></returns>
        List<SheepBind> GetSheepParentBind(string childId);

        /// <summary>
        /// 获取待流产羊绑定
        /// </summary>
        /// 母羊/种羊/近期已经交配过/交配后并未流产或者分娩
        /// <returns></returns>
        List<SheepBind> GetAbortionSheepBind();
        /// <summary>
        /// 获取待分娩羊绑定
        /// </summary>
        /// 母羊/种羊/近期已经交配过/交配后并未流产或者分娩
        /// <returns></returns>
        List<SheepBind> GetDeliverySheepBind();

        /// <summary>
        /// 配种羊只查询绑定
        /// </summary>
        /// <returns></returns>
        List<SheepBind> GetMatingSheepSelectBind();
        /// <summary>
        /// 配种羊只统计的搜索绑定
        /// </summary>
        /// <returns></returns>
        List<SheepBind> GetMatingSheepCountBind();

        /// <summary>
        /// 获取可交配的羊只的绑定
        /// 种羊与后备种羊（第二次鉴定后留下的）
        /// </summary>
        /// <returns></returns>
        List<SheepBind> GetStudSheepBind();

        List<SheepBind> GetStudSheepBindWithOuter();

        /// <summary>
        /// 流产羊只查询绑定
        /// </summary>
        /// <returns></returns>
        List<SheepBind> GetAbortionSheepSelectBind();
        /// <summary>
        /// 分娩羊只查询绑定
        /// </summary>
        /// <returns></returns>
        List<SheepBind> GetDeliverySheepSelectBind();
        /// <summary>
        /// 死亡羊只查询绑定
        /// </summary>
        /// <returns></returns>
        List<SheepBind> GetDeathSheepSelectBind();
        /// <summary>
        /// 第一次鉴定羊只查询绑定
        /// </summary>
        /// <returns></returns>
        List<SheepBind> GetFirstAssessSheepSelectBind();
        /// <summary>
        /// 种羊鉴定羊只搜索绑定
        /// </summary>
        /// <returns></returns>
        List<SheepBind> GetAssessStudSheepSelectBind();
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
        /// <summary>
        /// 一键排除鉴定羊只
        /// </summary>
        /// <returns></returns>
        List<SheepBind> GetExceptAssessSheepAddBind();
        List<SheepBind> GetExceptAssessSheepSelectBind();
        /// <summary>
        /// 疾病治疗羊只绑定
        /// </summary>
        /// <returns></returns>
        List<SheepBind> GetTreatmentSheepBind();

        List<BreedBind> GetBreedBind();

        List<SheepfoldBind> GetSheepfoldBind();
        /// <summary>
        /// 获取圈舍以及圈舍内的羊只数量
        /// </summary>
        /// <returns></returns>
        List<SheepfoldSheepCountBind> GetSheepfoldSheepCountBind();

        /// <summary>
        /// 在职员工绑定
        /// </summary>
        /// <returns></returns>
        List<EmployeeBind> GetEmployeeBind();
        /// <summary>
        /// 在职员工绑定 包含default系统默认员工
        /// </summary>
        /// <returns></returns>
        List<EmployeeBind> GetEmployeeBindWithDefault();


        /// <summary>
        /// 所有员工绑定
        /// </summary>
        /// <returns></returns>
        List<EmployeeBind> GetAllEmployeeBind();



        List<UserBind> GetUserBind();

        List<DutyBind> GetDutyBind();

        /// <summary>
        /// 分组的羊圈与羊只
        /// </summary>
        /// <returns></returns>
        Dictionary<SheepfoldBind, List<SheepBind>> GetMoveSheepfoldBind();

        /// <summary>
        /// 分组的疾病与症状
        /// </summary>
        /// <returns></returns>
        Dictionary<DiseaseTypeBind, List<DiseaseBind>> GetDiseaseTypeBind();

        /// <summary>
        /// 症状ID搜索疾病（包含一种症状就算）
        /// </summary>
        /// <param name="symptomIds"></param>
        /// <returns></returns>
        List<DiseaseBind> GetDiseaseBindBySymptomIds(params string[] symptomIds);

        /// <summary>
        /// 根据疾病名称模糊搜索疾病
        /// </summary>
        /// <param name="diseaseName"></param>
        /// <returns></returns>
        List<DiseaseBind> GetDiseaseBindByName(string diseaseName);

        /// <summary>
        /// 查询症状分类
        /// </summary>
        /// <returns></returns>
        Dictionary<SymptomTypeBind, List<SymptomBind>> GetSymptomTypeBind();

        /// <summary>
        /// 查询症状
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        List<SymptomBind> GetSymptomBind(SymptomBindFilter filter);

        List<PurchaserBind> GetPurchaserBind();

        /// <summary>
        /// FeedName
        /// </summary>
        /// <returns></returns>
        List<FeedNameBind> GetFeedNameBind();
        /// <summary>
        /// FeedTypeName
        /// </summary>
        /// <returns></returns>
        List<FeedTypeBind> GetFeedTypeBind();
        List<FeedTypeBind> GetFeedTypeBind(string feedNameId);
        /// <summary>
        /// AreaName
        /// </summary>
        /// <returns></returns>
        List<AreaBind> GetAreaBind();
        List<AreaBind> GetAreaBind(string feedNameId, string typeId);

        /// <summary>
        /// MedicineName
        /// </summary>
        /// <returns></returns>
        List<MedicineBind> GetMedicineNameBind();

        /// <summary>
        /// 生产商 联系人
        /// 准备废弃
        /// </summary>
        /// <returns></returns>
        List<ManufactureBind> GetManufactureBind();
        List<ManufactureBind> GetManufactureBind(string medicineNameId);
        /// <summary>
        /// 生产商
        /// </summary>
        /// <returns></returns>
        List<DepartmentBind> GetDepartmentBind();
        List<DepartmentBind> GetDepartmentBind(string medicineNameId);
        /// <summary>
        /// 药品类型绑定
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 买羊时的羊只绑定(添加财务)
        /// </summary>
        /// <returns></returns>
        List<SheepBind> GetBuySheepBind4Add();
        /// <summary>
        /// 买羊时的羊只绑定(查询财务)
        /// </summary>
        /// <returns></returns>
        List<SheepBind> GetBuySheepBind4Select();

        List<BuyFeedBind> GetBuyFeedBind4Add();
        List<BuyMedicineBind> GetBuyMedicineBind4Add();
        List<BuyOtherBind> GetBuyOtherBind4Add();

        List<SellFeedBind> GetSellFeedBind4Add();
        List<SellOtherBind> GetSellOtherBind4Add();

        List<FormulaBind> GetFormulaBind(bool? isEnable = true);

        #endregion

        #region Chart

        /// <summary>
        /// 统计时间点各成长阶段羊只数量
        /// </summary>
        /// <param name="date"></param>
        /// <param name="Gender"></param>
        /// <returns></returns>
        List<PeriodsSheepGrowthStageCount> GetPeriodsSheepGrowthStageCount(DateTime date, string breedId, GenderEnum? gender = null);

        /// <summary>
        /// 获取一定时间段内出售的羊只数量
        /// 按照月份来分
        /// </summary>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        List<PeriodsSellSheepCount> GetPeriodsSellSheepCount(DateTime? dtStart = null, DateTime? dtEnd = null);

        #endregion

        #region BaseInfo

        List<Breed> GetBreed(BreedFilter filter);
        List<Breed> GetBreed(BreedFilter filter, int rowsCount);

        List<Sheepfold> GetSheepfold(SheepfoldFilter filter);
        List<Sheepfold> GetSheepfold(SheepfoldFilter filter, int rowsCount);

        List<Sheep> GetSheep(SheepFilter filter, int rowsCount);

        /// <summary>
        /// 不包含死亡的与出售的
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCounts"></param>
        /// <returns></returns>
        List<Sheep> GetSheep(SheepFilter filter, int pageIndex, int pageSize, out int totalCounts);
        /// <summary>
        /// 所有羊只
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCounts"></param>
        /// <returns></returns>
        List<Sheep> GetAllSheep(SheepFilter filter, int pageIndex, int pageSize, out int totalCounts);

        Sheep GetSheepById(string id);

        /// <summary>
        /// 添加羊只
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="breedId"></param>
        /// <param name="gender"></param>
        /// <param name="growthStage"></param>
        /// <param name="orgin">来源</param>
        /// <param name="birthWeight"></param>
        /// <param name="compatriotNumber">同胎羔羊数</param>
        /// <param name="birthDay"></param>
        /// <param name="fatherId"></param>
        /// <param name="motherId"></param>
        /// <param name="sheppfoldId">羊圈编号</param>
        /// <param name="status"></param>
        /// <param name="principalId">操作人</param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ServiceResult<string> AddSheep(string serialNumber, string breedId, GenderEnum gender, GrowthStageEnum growthStage, OriginEnum origin, float? birthWeight, int compatriotNumber, DateTime? birthDay, string fatherId, string motherId, string sheepfoldId, string principalId, string operatorId, string remark);

        /// <summary>
        /// 添加羊只（购入）
        /// </summary>
        /// <returns></returns>
        ServiceResult<string> AddSheep(string serialNumber, string breedId, GenderEnum gender, GrowthStageEnum growthStage, OriginEnum origin, float? birthWeight, int compatriotNumber, DateTime? birthDay, string fatherId,string fatherSerialNumber, string motherId,string motherSerialNumber, string sheepfoldId, string principalId, string operatorId, string remark, string buySource, decimal buyMoney, float? buyWeight, DateTime buyOperationDate, string buyPrincipalId, string buyRemark);

        /// <summary>
        /// 获取当前月出生羊只数量
        /// </summary>
        /// <returns></returns>
        int GetCurMonthBirthCount();

        ServiceResult<bool> UpdateSheep(string breedId, GenderEnum gender, GrowthStageEnum growthStage, OriginEnum origin, float? birthWeight, int compatriotNumber, DateTime? birthDay, float? ablactationWeight, DateTime? ablactationDate, string fatherId, string motherId, string sheepfoldId, string principalId, string remark, string id);

        /// <summary>
        /// 添加羊品种
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description">描述</param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ServiceResult<string> AddBreed(string name, string description, string operatorId, string remark);

        /// <summary>
        /// 添加圈舍
        /// </summary>
        /// <param name="name">圈舍</param>
        /// <param name="administrator">圈舍管理员</param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ServiceResult<string> AddSheepFold(string name, string administrator, string operatorId, string remark);

        /// <summary>
        /// 获取羊场信息
        /// </summary>
        /// <returns></returns>
        Farm GetFarm();
        Sheepfold GetSheepFoldById(string id);
        /// <summary>
        /// 更新圈舍信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="administrator"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ServiceResult<bool> UpdateSheepFold(string name, string administrator, string remark, string id);

        /// <summary>
        /// 编辑羊场信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="contacts"></param>
        /// <param name="phone"></param>
        /// <param name="address"></param>
        /// <param name="code"></param>
        /// <param name="businessScope">经营范围</param>
        /// <param name="qualifications">羊场资质</param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ServiceResult<bool> EditFarm(string name, string contacts, string phone, string address, string code, string businessScope, string qualifications, string remark);

        #endregion

        #region System

        ServiceResult<string> Login(string userName, string password);

        ServiceResult<bool> UpdatePassword(string oldPassword, string newPassword, string id);

        ServiceResult<bool> UpdatePassword(string password, string id);

        List<SheepParameter> GetSheepParameters();
        /// <summary>
        /// 更新羊只配置参数
        /// </summary>
        /// <param name="list">更新羊只参数提醒的集合</param>
        /// <returns></returns>
        ServiceResult<bool> UpdateSheepParameters(List<UpdateSheepParameter> list, int deliveryRangeDays);

        /// <summary>
        /// 获取饲料临界值
        /// </summary>
        /// <returns></returns>
        List<FeedCritical> GetFeedCritical();
        /// <summary>
        /// 获取药品临界值
        /// </summary>
        /// <returns></returns>
        List<MedicineCritical> GetMedicineCritical();
        /// <summary>
        /// 更新临界值
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        ServiceResult<bool> UpdateCritical(List<UpdateCritical> list);

        /// <summary>
        /// 获取待产提醒
        /// </summary>
        /// <returns></returns>
        List<PreDeliveryRemaindful> GetPreDeliveryRemaindful();
        /// <summary>
        /// 获取断奶提醒
        /// </summary>
        /// <returns></returns>
        List<PreAblactationRemaindful> GetPreAblactationRemaindful();
        /// <summary>
        /// 获取饲料临界值提醒
        /// </summary>
        /// <returns></returns>
        List<FeedRemaindful> GetFeedRemaindful();
        /// <summary>
        /// 获取药品临界值提醒
        /// </summary>
        /// <returns></returns>
        List<MedicineRemaindful> GetMedicineRemaindful();

        /// <summary>
        /// 提醒待产母羊编号
        /// </summary>
        /// <returns></returns>
        List<string> GetPreDeliverySerialNumber();
        /// <summary>
        /// 提醒待断奶羔羊编号
        /// </summary>
        /// <returns></returns>
        List<string> GetPreAblactationSerialNumber();


        /// <summary>
        /// 查询所有权限项
        /// </summary>
        /// <returns></returns>
        List<Permission> GetAllPermissions(PermissionFilter filter, int pageIndex, int pageSize, out int totalCount);

        /// <summary>
        /// 查询所有角色
        /// </summary>
        /// <returns></returns>
        List<Role> GetAllRoles(RoleFilter filter, int pageIndex, int pageSize, out int totalCount);

        /// <summary>
        /// 查询给定角色的所有权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        List<Permission> GetPermissionsByRoleId(string roleId);

        /// <summary>
        /// 查询给定的用户的所有权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        List<Permission> GetAllPermissionByUserId(string userId);

        /// <summary>
        /// 查询给定的用户的权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<Permission> GetPermissionByUserId(string userId);

        /// <summary>
        /// 查询给定用户的所有角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<Role> GetAllRoleByUserId(string userId);

        /// <summary>
        /// 查询角色尚未分配的权限
        /// </summary>
        /// <param name="keyWord">权限名称，描述检索关键字</param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        List<Permission> GetRoleAvailablePermissions(string keyWord, string roleId);

        /// <summary>
        /// 查询用户尚未分配的权限
        /// </summary>
        /// <param name="keyWord">权限名称，描述检索关键字</param>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<Permission> GetUserAvailablePermissions(string keyWord, string userId);

        /// <summary>
        /// 查询用户尚未分配的角色
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<Role> GetUserAvailableRoles(string keyWord, string userId);

        /// <summary>
        /// 增加角色
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ServiceResult<string> AddRole(string name, string description, string operatorId, string remark);

        /// <summary>
        /// 增加权限
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="URL">路径，类似于LSideBar.xml</param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ServiceResult<string> AddPermission(string name, string description, string URL, string operatorId, string remark);

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ServiceResult<string> AddUser(string userName, string password, string operatorId, string remark);

        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ServiceResult<bool> UpdateRole(string name, string description, string remark, string id);

        /// <summary>
        /// 编辑权限
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ServiceResult<bool> UpdatePermission(string name, string description,  string remark, string id);

        /// <summary>
        /// 根据角色Id获取角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Role GetRoleById(string id);

        /// <summary>
        /// 根据权限Id获取权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Permission GetPermissionById(string id);

        /// <summary>
        /// 给角色赋权限
        /// </summary>
        /// <param name="permissionIds"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        ServiceResult<bool> GrantPermission2Role(List<string> permissionIds, string roleId);

        /// <summary>
        /// 给用户赋角色
        /// </summary>
        /// <param name="roleIds"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        ServiceResult<bool> GrantRole2User(List<string> roleIds, string userId);

        /// <summary>
        /// 给用户给权限
        /// </summary>
        /// <param name="permissionIds"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        ServiceResult<bool> GrantPermission2User(List<string> permissionIds, string userId);

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ServiceResult<bool> DeletePermission(string id);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ServiceResult<bool> DeleteRole(string id);

        //-------------分场-------------

        /// <summary>
        /// 增加分场
        /// </summary>
        /// <param name="name"></param>
        /// <param name="manager"></param>
        /// <returns></returns>
        ServiceResult<string> AddSplitYard(string name, string manager);

        /// <summary>
        /// 查询分场
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<SplitYard> GetAllSplitYard(SplitYardFilter filter, int pageIndex, int pageSize, out int totalCount);

        /// <summary>
        /// 编辑分场
        /// </summary>
        /// <param name="name"></param>
        /// <param name="manager"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        ServiceResult<bool> UpdateSplitYard(string name, string manager, string id);

        /// <summary>
        /// 根据Id获取分场
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SplitYard GetSplitYardById(string id);

        #endregion

        #region Multiplying 繁殖育种

        /// <summary>
        /// 交配记录
        /// </summary>
        /// <param name="femaleId">母羊</param>
        /// <param name="maleId">公羊</param>
        /// <param name="matingDate"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ServiceResult<string> AddMating(string femaleId, string maleId, DateTime matingDate, string principalId, string operatorId, string remark);
        /// <summary>
        /// 添加分娩记录
        /// </summary>
        /// <param name="femaleId"></param>
        /// <param name="deliveryWay"></param>
        /// <param name="DeliverReason">助产原因</param>
        /// <param name="DeliverReasonOtherDetail">助产原因其他具体原因</param>
        /// <param name="liveMaleCount">产活公羔数</param>
        /// <param name="liveFemaleCount">产活羔数</param>
        /// <param name="totalCount">总产羔数</param>
        /// <param name="deliveryDate"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ServiceResult<string> AddDelivery(string femaleId, DeliveryWayEnum deliveryWay, MidwiferyReasonEnum deliverReason, string deliverReasonOtherDetail, int? liveMaleCount, int? liveFemaleCount, int totalCount, DateTime deliveryDate, string principalId, string operatorId, string remark, List<Chanyi.Shepherd.QueryModel.AddModel.BaseInfo.Sheep> lambList);


        /// <summary>
        /// 不再提醒预产期
        /// </summary>
        /// <param name="maleId"></param>
        /// <returns></returns>
        ServiceResult<bool> UpdateMatingDisRemindful(string maleId);
        /// <summary>
        /// 获取分页交配记录
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCounts"></param>
        /// <returns></returns>
        List<Mating> GetMating(MatingFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<Mating> GetMating(MatingFilter filter, int rowsCount);
        Mating GetMatingById(string id);
        ServiceResult<bool> DeleteMating(string id);

        List<MatingCount> GetMatingCount(string sheepId, int? count, int? year, SeasonEnum? season, int pageIndex, int pageSize, out int totalCount);
        List<MatingCount> GetMatingCount(string sheepId, int? count, int? year, SeasonEnum? season, int rowsCount);


        /// <summary>
        /// 流产记录
        /// </summary>
        /// <param name="sheepId">流产母羊</param>
        /// <param name="reason">流产原因</param>
        /// <param name="dispose">处理方式</param>
        /// <param name="abortionDate">流产时间</param>
        /// <param name="principalId">处理人</param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ServiceResult<string> AddAbortion(string sheepId, string reason, string dispose, DateTime abortionDate, string principalId, string operatorId, string remark);

        /// <summary>
        /// 断奶管理
        /// </summary>
        /// <param name="sheepId"></param>
        /// <param name="ablactationWeight">断奶重量</param>
        /// <param name="ablactationDate">断奶日期</param>
        /// <param name="principalId">操作人</param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ServiceResult<string> AddAblactation(string sheepId, float ablactationWeight, DateTime ablactationDate, string principalId, string operatorId, string remark);

        /// <summary>
        /// 分娩
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<Delivery> GetDelivery(DeliveryFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<Delivery> GetDelivery(DeliveryFilter filter, int rowsCount);
        Delivery GetDeliveryById(string id);
        ServiceResult<bool> DeleteDelivery(string id);

        /// <summary>
        /// 流产
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<Abortion> GetAbortion(AbortionFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<Abortion> GetAbortion(AbortionFilter filter, int rowsCount);
        Abortion GetAbortionById(string id);
        ServiceResult<bool> DeleteAbortion(string id);

        /// <summary>
        /// 断奶
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<Ablactation> GetAblactation(AblactationFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<Ablactation> GetAblactation(AblactationFilter filter, int rowsCount);
        Ablactation GetAblactationById(string id);

        /// <summary>
        /// 编辑配种记录的接口
        /// </summary>
        /// <param name="femaleId"></param>
        /// <param name="maleId"></param>
        /// <param name="matingDate"></param>
        /// <param name="principalId"></param>
        /// <param name="remark"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        ServiceResult<bool> UpdateMating(string femaleId, string maleId, DateTime matingDate, string principalId, string remark, string id);

        /// <summary>
        /// 编辑分娩
        /// </summary>
        /// <param name="deliveryWay"></param>
        /// <param name="lambCount"></param>
        /// <param name="liveCount"></param>
        /// <param name="deliveryDate"></param>
        /// <param name="principalId"></param>
        /// <param name="remark"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        ServiceResult<bool> UpdateDelivery(DeliveryWayEnum deliveryWay, MidwiferyReasonEnum deliverReason, string deliverReasonOtherDetail, int? liveMaleCount, int? liveFemaleCount, int totalCount, DateTime deliveryDate, string principalId, string remark, string id);

        /// <summary>
        /// 编辑流产
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="dispose"></param>
        /// <param name="abortionDate"></param>
        /// <param name="principalId"></param>
        /// <param name="remark"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        ServiceResult<bool> UpdateAbortion(string reason, string dispose, DateTime abortionDate, string principalId, string remark, string id);

        /// <summary>
        /// 编辑断奶
        /// </summary>
        /// <param name="ablactationWeight"></param>
        /// <param name="ablactationDate"></param>
        /// <param name="sheepId"></param>
        /// <param name="principalId"></param>
        /// <param name="remark"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        ServiceResult<bool> UpdateAblactation(float ablactationWeight, DateTime ablactationDate, string sheepId, string principalId, string remark, string id);

        #endregion

        #region Breeding 鉴定

        //种羊还是种羊
        //第一次是断奶
        //第二次是配种前
        //第三次是产后

        /// <summary>
        /// 种羊鉴定
        /// </summary>
        /// <param name="studSheepId">种羊</param>
        /// <param name="matingAbility">交配能力</param>
        /// <param name="weight">种羊重量</param>
        /// <param name="habitusScore">体型评分</param>
        /// <param name="assessDate">鉴定时间</param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ServiceResult<string> AddAssessStudsheep(string studSheepId, float matingAbility, float weight, float habitusScore, DateTime assessDate, string principalId, string operatorId, string remark);

        /// <summary>
        /// 第一次鉴定
        /// </summary>
        /// <param name="sheepId">第一次鉴定的羊</param>
        /// <param name="weight">体重</param>
        /// <param name="habitusScore">体型评分</param>
        /// <param name="assessDate">鉴定时间</param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ServiceResult<string> AddFirstAssess(string sheepId, float weight, float habitusScore, DateTime assessDate, string principalId, string operatorId, string remark);

        /// <summary>
        /// 第二次鉴定
        /// </summary>
        /// <param name="sheepId">第二次鉴定的羊</param>
        /// <param name="BreedFeatureScore">品种特征评分</param>
        /// <param name="GenitaliaScore">生殖器官评分</param>
        /// <param name="weight"></param>
        /// <param name="habitusScore"></param>
        /// <param name="assessDate"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ServiceResult<string> AddSecondAssess(string sheepId, float breedFeatureScore, float genitaliaScore, float weight, float habitusScore, DateTime assessDate, string principalId, string operatorId, string remark);


        /// <summary>
        /// 第三次鉴定
        /// </summary>
        /// <param name="sheepId">育成羊</param>
        /// <param name="MatingAbility">配种能力</param>
        /// <param name="weight"></param>
        /// <param name="habitusScore"></param>
        /// <param name="assessDate"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ServiceResult<string> AddThirdAssess(string sheepId, float matingAbility, float weight, float habitusScore, DateTime assessDate, string principalId, string operatorId, string remark);
        /// <summary>
        /// 添加一键否决羊只
        /// </summary>
        /// <param name="sheepId"></param>
        /// <param name="reason"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ServiceResult<string> AddExceptAssessSheep(string sheepId, string reason, string principalId, string operatorId, string remark);

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
        /// <summary>
        /// 一键否决
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<ExceptAssess> GetExceptAssess(ExceptAssessFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<ExceptAssess> GetExceptAssess(ExceptAssessFilter filter, int rowCount);

        ServiceResult<bool> DeleteExceptAssessSheep(string id);

        ServiceResult<bool> UpdateAssessStudsheep(float matingAbility, float weight, float habitusScore, DateTime assessDate, string principalId, string remark, string id);

        ServiceResult<bool> UpdateFirstAssess(float weight, float habitusScore, DateTime assessDate, string principalId, string remark, string id);

        ServiceResult<bool> UpdateSecondAssess(float breedFeatureScore, float genitaliaScore, float weight, float habitusScore, DateTime assessDate, string principalId, string remark, string id);

        ServiceResult<bool> UpdateThirdAssess(float matingAbility, float weight, float habitusScore, DateTime assessDate, string principalId, string remark, string id);

        #endregion

        #region HR

        /// <summary>
        /// 添加员工
        /// </summary>
        /// <param name="name"></param>
        /// <param name="gender"></param>
        /// <param name="idNum">身份证</param>
        /// <param name="entryDate">入职时间</param>
        /// <param name="salary">工资</param>
        /// <param name="sariaNum">工号</param>
        /// <param name="dutyID">职务</param>
        /// <param name="status">状态</param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ServiceResult<string> AddEmployee(string name, GenderEnum gender, string idNum, DateTime entryDate, decimal salary, string serialNum, string dutyId, EmployeeStatusEnum status, string principalId, string operatorId, string remark);

        List<Employee> GetEmployee(EmployeeFilter filter, int pageIndex, int pageSize, out int totalCount);

        List<Employee> GetEmployee(EmployeeFilter filter, int rowsCount);
        Employee GetEmployeeById(string id);

        ServiceResult<bool> UpdateEmployee(string name, GenderEnum gender, string idNum, DateTime entryDate, decimal salary, string serialNum, string dutyId, EmployeeStatusEnum status, string principalId, string remark, string id);
        /// <summary>
        /// 员工离职
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="reason"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ServiceResult<string> AddQuit(string employeeId, string reason, DateTime quitDate, string principalId, string operatorId, string remark);
        List<Quit> GetQuit(QuitFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<Quit> GetQuit(QuitFilter filter, int rowsCount);

        ServiceResult<string> AddDuty(string name, string description, string operatorId, string remark);

        List<User> GetUser(int pageIndex, int pageSize, out int totalCount);
        List<User> GetUser(int rowsCount);
        User GetUserById(string id);

        ServiceResult<bool> UpdateUser(bool isEnabled, string remark, string id);

        List<Cooperater> GetCooperater(CooperaterFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<Cooperater> GetCooperater(CooperaterFilter filter, int rowsCount);
        Cooperater GetCooperaterById(string id);

        ServiceResult<bool> UpdatePurchaser(string name, string department, string contactInfo, string principalId, string remark, string id);

        ServiceResult<string> AddPurchaser(string name, string department, string contactInfo, string operatorId, string principalId, string remark);

        #endregion

        #region GroupManage 种群管理

        /// <summary>
        /// 新增转圈记录
        /// </summary>
        /// <param name="sheepIds">转圈羊只集合</param>
        /// <param name="targetSheepfold">目标圈舍ID</param>
        /// <returns></returns>
        ServiceResult<bool> AddMoveSheepfold(IEnumerable<string> sheepIds, string targetSheepfold, string principalId, string operatorId, string remark);

        /// <summary>
        /// 死亡管理
        /// </summary>
        /// <param name="sheepId"></param>
        /// <param name="reason">死亡原因</param>
        /// <param name="dispose">处理方式</param>
        /// <param name="deathDate">死亡日期</param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ServiceResult<string> AddDeathManage(string sheepId, string reason, DeathDisposeEnum dispose, DateTime deathDate, string principalId, string operatorId, string remark);

        List<DeathManage> GetDeathManage(DeathManageFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<DeathManage> GetDeathManage(DeathManageFilter filter, int rowsCount);
        DeathManage GetDeathManageById(string id);
        ServiceResult<bool> DeleteDeathManage(string id);

        ServiceResult<bool> UpdateDeathManage(string reason, DeathDisposeEnum dispose, DateTime deathDate, string principalId, string remark, string id);

        List<MoveSheepfold> GetMoveSheepfold(MoveSheepfoldFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<MoveSheepfold> GetMoveSheepfold(MoveSheepfoldFilter filter, int rowsCount);

        #endregion

        #region Assist

        ///// <summary>
        ///// 疾病类型,pid已经从数据库删除
        ///// </summary>
        ///// <param name="pid">可空，默认取第一级疾病</param>
        ///// <returns></returns>
        //List<DiseaseType> GetDiseaseType(string pid = "0");

        ///// <summary>
        ///// 获取疾病
        ///// </summary>
        ///// <param name="typeId">疾病类型</param>
        ///// <returns></returns>
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
        //List<Disease> GetCrossDiseaseBySymptomIds(params string[] symptomIds);

        #endregion

        #region Formula 配方

        List<Formula> GetFormula(FormulaFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<Formula> GetFormula(FormulaFilter filter, int rowsCount);
        /// <summary>
        /// 配方对应的饲料列表
        /// </summary>
        /// <param name="formulaId"></param>
        /// <returns></returns>
        List<FormulaFeed> GetFormulaFeedById(string formulaId);

        /// <summary>
        /// 获取饲料Id,Name,Area,Type
        /// </summary>
        /// <returns></returns>
        List<SimpleFeed> GetSimpleFeed();

        /// <summary>
        /// 配方参考标准
        /// </summary>
        /// <returns></returns>
        List<FormulaNutrient> GetFormulaNutrient(FormulaNutrientFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<FormulaNutrient> GetFormulaNutrient(FormulaNutrientFilter filter, int rowsCount);
        FormulaNutrient GetFormulaNutrientById(string id);

        /// <summary>
        /// 设置配方状态
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        ServiceResult<bool> UpdateFormulaStatus(bool isEnable, string id);

        ServiceResult<string> AddFormulaNutrient(string name, float? dailyGain, float? CP, float? DMI, float? EE, float? CF, float? NFE, float? Ash, float? NDF, float? ADF, float? Starch, float? Ga, float? AllP, float? Arg, float? His, float? Ile, float? Leu, float? Lys, float? Met, float? Cys, float? Phe, float? Tyr, float? Thr, float? Trp, float? Val, float? P, float? Na, float? Cl, float? Mg, float? K, float? Fe, float? Cu, float? Mn, float? Zn, float? Se, float? Carotene, float? VE, float? VB1, float? VB2, float? PantothenicAcid, float? Niacin, float? Biotin, float? Folic, float? Choline, float? VB6, float? VB12, float? LinoleicAcid, float? Salt, string principalId, string operatorId, string remark);

        ServiceResult<bool> UpdateFormulaNutrient(string name, float? dailyGain, float? CP, float? DMI, float? EE, float? CF, float? NFE, float? Ash, float? NDF, float? ADF, float? Starch, float? Ga, float? AllP, float? Arg, float? His, float? Ile, float? Leu, float? Lys, float? Met, float? Cys, float? Phe, float? Tyr, float? Thr, float? Trp, float? Val, float? P, float? Na, float? Cl, float? Mg, float? K, float? Fe, float? Cu, float? Mn, float? Zn, float? Se, float? Carotene, float? VE, float? VB1, float? VB2, float? PantothenicAcid, float? Niacin, float? Biotin, float? Folic, float? Choline, float? VB6, float? VB12, float? LinoleicAcid, float? Salt, string principalId, string remark, string id);

        /// <summary>
        /// 添加配方
        /// </summary>
        /// <param name="formulaFeed">饲料与用量键值对</param>
        /// <param name="name"></param>
        /// <param name="applyTo">适用于</param>
        /// <param name="sideEffect">不良反应</param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ServiceResult<string> AddFormula(Dictionary<string, float> formulaFeed, string name, string applyTo, string sideEffect, string principalId, string operatorId, string remark);

        //ServiceResult<bool> DeleteFormula(string id);

        #endregion

        #region Inputs

        #region 饲料
        /// <summary>
        /// 饲料出入库
        /// </summary>
        /// <param name="nameId">组合主键</param>
        /// <param name="typeId">组合主键</param>
        /// <param name="areaId">组合主键</param>
        /// <param name="amount"></param>
        /// <param name="operationDate"></param>
        /// <param name="direction">进还是出</param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <param name="dispositon">出库去向</param>
        /// <returns></returns>
        ServiceResult<string> AddFeedInOutWarehouse(string nameId, string typeId, string areaId, float amount, DateTime operationDate, InOutWarehouseDirectionEnum direction, string principalId, string operatorId, string remark, OutWarehouseDispositonEnum? dispositon = OutWarehouseDispositonEnum.None);

        /// <summary>
        /// 批量饲料出库（自定义配方出库或者批量出库）
        /// </summary>
        /// <param name="dictFeedAmount">饲料、用量</param>
        /// <param name="shepfolds">圈舍集合</param>
        /// <param name="operationDate"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ServiceResult<int> AddFeedBatchOutWarehouse(Dictionary<string, float> dictFeedAmount, List<string> shepfolds, DateTime operationDate, string principalId, string operatorId, string remark);

        ServiceResult<string> AddAreaName(string name, string operatorId, string remark);

        ServiceResult<string> AddFeedName(string name, string operatorId, string remark);

        ServiceResult<string> AddFeed(string feedNameId, string typeNameId, string areaId, string description, string operatorId, float? CP, float? DMI, float? EE, float? CF, float? NFE, float? Ash, float? NDF, float? ADF, float? Starch, float? Ga, float? Arg, float? His, float? Ile, float? Leu, float? Lys, float? Met, float? Cys, float? Phe, float? Tyr, float? Thr, float? Trp, float? Val, float? P, float? Na, float? Cl, float? Mg, float? K, float? Fe, float? Cu, float? Mn, float? Zn, float? Se, float? Carotene, float? VE, float? VB1, float? VB2, float? PantothenicAcid, float? Niacin, float? Biotin, float? Folic, float? Choline, float? VB6, float? VB12, float? LinoleicAcid, float? AllP);

        /// <summary>
        /// 获取饲料详情
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<Feed> GetFeed(FeedFilter filter, int pageIndex, int pageSize, out int totalCount);

        List<Feed> GetFeed(FeedFilter filter, int rowCount);
        Feed GetFeedByKindId(string kindId);

        /// <summary>
        /// 获取一种饲料的详情
        /// </summary>
        /// <param name="kindId"></param>
        /// <returns></returns>
        FeedDetail GetFeedDetail(string kindId);

        /// <summary>
        /// 获取饲料出入库
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<FeedInOut> GetFeedInOut(FeedInOutFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<FeedInOut> GetFeedInOut(FeedInOutFilter filter, int rowCount);

        /// <summary>
        /// 查询指定出入库条目的详情
        /// </summary>
        /// <param name="id">linkId</param>
        /// <returns></returns>
        FeedInOut GetFeedInOutDetailById(string id);

        /// <summary>
        /// 获取饲料库存
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<FeedInventory> GetFeedInventory(FeedInventoryFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<FeedInventory> GetFeedInventory(FeedInventoryFilter filter, int rowsCount);
        List<FeedInventory> GetFeedInventory();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="feedNameId"></param>
        /// <param name="typeNameId"></param>
        /// <param name="areaId"></param>
        /// <param name="description"></param>
        /// <param name="operatorId"></param>
        /// <param name="CP"></param>
        /// <param name="DMI"></param>
        /// <param name="EE"></param>
        /// <param name="CF"></param>
        /// <param name="NFE"></param>
        /// <param name="Ash"></param>
        /// <param name="NDF"></param>
        /// <param name="ADF"></param>
        /// <param name="Starch"></param>
        /// <param name="Ga"></param>
        /// <param name="Arg"></param>
        /// <param name="His"></param>
        /// <param name="Ile"></param>
        /// <param name="Leu"></param>
        /// <param name="Lys"></param>
        /// <param name="Met"></param>
        /// <param name="Cys"></param>
        /// <param name="Phe"></param>
        /// <param name="Tyr"></param>
        /// <param name="Thr"></param>
        /// <param name="Trp"></param>
        /// <param name="Val"></param>
        /// <param name="P"></param>
        /// <param name="Na"></param>
        /// <param name="Cl"></param>
        /// <param name="Mg"></param>
        /// <param name="K"></param>
        /// <param name="Fe"></param>
        /// <param name="Cu"></param>
        /// <param name="Mn"></param>
        /// <param name="Zn"></param>
        /// <param name="Se"></param>
        /// <param name="Carotene"></param>
        /// <param name="VE"></param>
        /// <param name="VB1"></param>
        /// <param name="VB2"></param>
        /// <param name="PantothenicAcid"></param>
        /// <param name="Niacin"></param>
        /// <param name="Biotin"></param>
        /// <param name="Folic"></param>
        /// <param name="Choline"></param>
        /// <param name="VB6"></param>
        /// <param name="VB12"></param>
        /// <param name="LinoleicAcid"></param>
        /// <param name="AllP"></param>
        /// <param name="id">kindId(FeedId)</param>
        /// <returns></returns>
        ServiceResult<bool> UpdateFeed(string feedNameId, string typeNameId, string areaId, string description, string operatorId, float? CP, float? DMI, float? EE, float? CF, float? NFE, float? Ash, float? NDF, float? ADF, float? Starch, float? Ga, float? Arg, float? His, float? Ile, float? Leu, float? Lys, float? Met, float? Cys, float? Phe, float? Tyr, float? Thr, float? Trp, float? Val, float? P, float? Na, float? Cl, float? Mg, float? K, float? Fe, float? Cu, float? Mn, float? Zn, float? Se, float? Carotene, float? VE, float? VB1, float? VB2, float? PantothenicAcid, float? Niacin, float? Biotin, float? Folic, float? Choline, float? VB6, float? VB12, float? LinoleicAcid, float? AllP, string id);

        //根据配方Id查询饲料相关度
        //List<FeedFormulaRelevancy> GetFeedFormulaRelevancy(string formulaId);

        /// <summary>
        /// 查询所有的饲料，包含非关键性字段
        /// </summary>
        /// <returns></returns>
        List<FeedWithAllFileds> GetFeedWithAllFileds();

        #endregion

        #region 药品
        /// <summary>
        /// 药品出入库
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="medicineId"></param>
        /// <param name="expirationDate">过期时间</param>
        /// <param name="amount"></param>
        /// <param name="manufacturerId">生产商</param>
        /// <param name="direction"></param>
        /// <param name="operationDate"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <param name="dispositon">出库去向</param>
        /// <returns></returns>
        ServiceResult<string> AddMedicineInOutWarehouse(string nameId, string manufacturerId, string typeId, DateTime expirationDate, float amount, InOutWarehouseDirectionEnum direction, DateTime operationDate, string principalId, string operatorId, string remark);
        ServiceResult<string> AddMedicineName(string name, string operatorId, string remark);
        /// <summary>
        /// 添加供应商
        /// </summary>
        /// <param name="name"></param>
        /// <param name="department"></param>
        /// <param name="contactInfo"></param>
        /// <param name="operatorId"></param>
        /// <param name="principalId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ServiceResult<string> AddManufacturer(string name, string department, string contactInfo, string operatorId, string principalId, string remark);
        /// <summary>
        /// 添加药品
        /// </summary>
        /// <param name="nameId"></param>
        /// <param name="manufacturerId">供货商</param>
        /// <param name="typeId">类型</param>
        /// <param name="medicineUnit">单位</param>
        /// <param name="operatorId"></param>
        /// <param name="principalId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ServiceResult<string> AddMedicine(string nameId, string manufacturerId, string typeId, string medicineUnit, string operatorId, string remark);

        List<Medicine> GetMedicine(MedicineFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<Medicine> GetMedicine(MedicineFilter filter, int rowsCount);
        Medicine GetMedicineByKindId(string kindId);

        /// <summary>
        /// 药品出入库
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<MedicineInOut> GetMenicineInOut(MedicineInOutFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<MedicineInOut> GetMenicineInOut(MedicineInOutFilter filter, int rowsCount);
        MedicineInOut GetMenicineInOutDetailById(string id);

        /// <summary>
        /// 药品库存
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<MedicineInventory> GetMedicineInventory(MedicineInventoryFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<MedicineInventory> GetMedicineInventory(MedicineInventoryFilter filter, int rowsCount);

        ServiceResult<bool> UpdateMedicine(string nameId, string manufacturerId, string typeId, string remark, string id);

        #endregion

        #region 其他

        /// <summary>
        /// 其他物品出入库
        /// </summary>
        /// <param name="nameId">实际是kindId</param>
        /// <param name="amount"></param>
        /// <param name="unit"></param>
        /// <param name="direction"></param>
        /// <param name="operationDate"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <param name="dispositon">出库去向</param>
        /// <returns></returns>
        ServiceResult<string> AddOtherInOutWarehouse(string nameId, float amount, string unit, InOutWarehouseDirectionEnum direction, DateTime operationDate, string principalId, string operatorId, string remark, OutWarehouseDispositonEnum? dispositon = OutWarehouseDispositonEnum.None);

        ServiceResult<string> AddOther(string name, string unit, string operatorId, string remark);

        List<Other> GetOther(OtherFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<Other> GetOther(OtherFilter filter, int rowCount);
        Other GetOtherByKindId(string kindId);

        /// <summary>
        /// 药品出入库
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<OtherInOut> GetOtherInOut(OtherInOutFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<OtherInOut> GetOtherInOut(OtherInOutFilter filter, int rowCount);
        OtherInOut GetOtherInOutDetailById(string id);

        /// <summary>
        /// 药品库存
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<OtherInventory> GetOtherInventory(OtherInventoryFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<OtherInventory> GetOtherInventory(OtherInventoryFilter filter, int rowCount);

        ServiceResult<bool> UpdateOther(string name, string unit, string remark, string id);


        #endregion

        #endregion

        #region Finance

        #region 卖
        /// <summary>
        /// 出售羊只
        /// </summary>
        /// <param name="list">羊只Id、价格、重量</param>
        /// <param name="purchaserId"></param>
        /// <param name="operationDate"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns>返回值没有实际意义</returns>
        ServiceResult<int> AddSellSheep(List<AddSellSheep> list, decimal totalPrice, float totalWeight, string purchaserId, DateTime operationDate, string principalId, string operatorId, string remark);

        /// <summary>
        /// 获取出售羊只批次
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="TotalCount"></param>
        /// <returns></returns>
        List<SellSheepBatch> GetSellSheepBath(SellSheepBatchFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<SellSheepBatch> GetSellSheepBath(SellSheepBatchFilter filter, int rowsCount);


        /// <summary>
        /// 群体管理只是负责展示
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<SellSheep> GetSellSheep(SellSheepFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<SellSheep> GetSellSheep(SellSheepFilter filter, int rowsCount);

        /// <summary>
        /// 根据批次获取出售羊只
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        List<SellSheep> GetSellSheep(string batchId, int pageIndex, int pageSize, out int totalCount);

        /// <summary>
        /// 买大粪
        /// </summary>
        /// <param name="price"></param>
        /// <param name="purchaserId"></param>
        /// <param name="operationDate"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark">备注中填写具体数量</param>
        /// <returns></returns>
        ServiceResult<string> AddSellManure(decimal price, string purchaserId, DateTime operationDate, string principalId, string operatorId, string remark);

        List<SellManure> GetSellManure(SellManureFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<SellManure> GetSellManure(SellManureFilter filter, int rowsCount);

        /// <summary>
        /// 卖羊毛
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="price"></param>
        /// <param name="purchaserId"></param>
        /// <param name="operationDate"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ServiceResult<string> AddSellWool(float amount, decimal price, string purchaserId, DateTime operationDate, string principalId, string operatorId, string remark);

        List<SellWool> GetSellWool(SellWoolFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<SellWool> GetSellWool(SellWoolFilter filter, int rowsCount);

        ServiceResult<int> AddSellFeed(List<AddSellInput> list, string operatorId, string remark);
        List<SellFeed> GetSellFeed(SellFeedFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<SellFeed> GetSellFeed(SellFeedFilter filter, int rowsCount);

        ServiceResult<int> AddSellOther(List<AddSellInput> list, string operatorId, string remark);
        List<SellOther> GetSellOther(SellOtherFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<SellOther> GetSellOther(SellOtherFilter filter, int rowsCount);

        #endregion

        #region 买

        /// <summary>
        /// 交电费
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="money"></param>
        /// <param name="operationDate"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ServiceResult<string> AddElectricCharge(float amount, decimal money, DateTime operationDate, string principalId, string operatorId, string remark);
        List<ElectricCharge> GetElectricCharge(ElectricChargeFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<ElectricCharge> GetElectricCharge(ElectricChargeFilter filter, int rowsCount);

        ServiceResult<string> AddWaterRate(float amount, decimal money, DateTime operationDate, string principalId, string operatorId, string remark);
        List<WaterRate> GetWaterRate(WaterRateFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<WaterRate> GetWaterRate(WaterRateFilter filter, int rowsCount);

        ServiceResult<string> AddPayoff(string employeeId, decimal money, DateTime operationDate, string principalId, string operatorId, string remark);
        List<Payoff> GetPayoff(PayoffFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<Payoff> GetPayoff(PayoffFilter filter, int rowsCount);

        ServiceResult<string> AddIncidentals(decimal money, DateTime operationDate, string principalId, string operatorId, string remark);
        List<Incidentals> GetIncidentals(IncidentalsFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<Incidentals> GetIncidentals(IncidentalsFilter filter, int rowsCount);

        ServiceResult<string> AddBuySheep(string sheepId, string source, decimal money, DateTime operationDate, string principalId, string operatorId, string remark, float? weight);
        List<BuySheep> GetBuySheep(BuySheepFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<BuySheep> GetBuySheep(BuySheepFilter filter, int rowsCount);

        /// <summary>
        /// 购买饲料
        /// </summary>
        /// <param name="inputExpenditure">linkId与money</param>
        /// <param name="operationDate"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns>受影响行数（-1为异常）</returns>
        ServiceResult<int> AddBuyFeed(Dictionary<string, decimal> inputExpenditure, DateTime operationDate, string principalId, string operatorId, string remark);
        List<BuyFeed> GetBuyFeed(BuyFeedFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<BuyFeed> GetBuyFeed(BuyFeedFilter filter, int rowsCount);

        /// <summary>
        /// 购买药品
        /// </summary>
        /// <param name="inputExpenditure">linkId与money</param>
        /// <param name="operationDate"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns>受影响行数（-1为异常）</returns>
        ServiceResult<int> AddBuyMedicine(Dictionary<string, decimal> inputExpenditure, DateTime operationDate, string principalId, string operatorId, string remark);
        List<BuyMedicine> GetBuyMedicine(BuyMedicineFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<BuyMedicine> GetBuyMedicine(BuyMedicineFilter filter, int rowsCount);

        /// <summary>
        /// 添加购买其他
        /// </summary>
        /// <param name="inputExpenditure">linkId与money</param>
        /// <param name="operationDate"></param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns>受影响行数（-1为异常）</returns>
        ServiceResult<int> AddBuyOther(Dictionary<string, decimal> inputExpenditure, DateTime operationDate, string principalId, string operatorId, string remark);
        List<BuyOther> GetBuyOther(BuyOtherFilter filter, int pageIndex, int pageSize, out int totalCount);
        List<BuyOther> GetBuyOther(BuyOtherFilter filter, int rowsCount);

        #endregion

        #endregion

        #region DecisionAids 决策辅助

        /// <summary>
        /// 系谱
        /// </summary>
        /// <param name="sheepId">羊只Id</param>
        /// <param name="depth">查询深度（向上几代）</param>
        /// <returns></returns>
        List<FamilyTree> GetFamilyTree(string sheepId, int? depth = 5);
        /// <summary>
        /// 辅助配种
        /// </summary>
        /// <param name="sheepId">羊只Id</param>
        /// <param name="depth">到第几代</param>
        /// <returns></returns>
        List<AssistMating> GetAssistMating(string sheepId, int depth);

        /// <summary>
        /// 验证两只羊可否配种
        /// </summary>
        /// <param name="maleId"></param>
        /// <param name="femaleId"></param>
        /// <returns></returns>
        ServiceResult<bool> VarifyTwoSheepsCanMating(string maleId, string femaleId);

        #endregion

        #region DiseaseControl 疾病防疫

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
        ServiceResult<string> AddAntiepidemic(string name, string vaccine, DateTime executeDate, string effect, string sheepFlock, string principalId, string operatorId, string remark);

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
        ServiceResult<bool> UpdateAntiepidemic(string name, string vaccine, DateTime executeDate, string effect, string sheepFlock, string principalId, string remark, string id);

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
        ServiceResult<string> AddAntiepidemicPlan(string name, string vaccine, DateTime planExecuteDate, string sheepFlock, string principalId, string operatorId, string remark);
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
        ServiceResult<bool> UpdateAntiepidemicPlan(string name, string vaccine, DateTime planExecuteDate, string sheepFlock, string principalId, string remark, string id);

        /// <summary>
        /// 执行防疫计划
        /// </summary>
        /// <param name="planId">防疫计划Id</param>
        /// <param name="executeDate">执行时间</param>
        /// <param name="effect">执行效果</param>
        /// <param name="principalId"></param>
        /// <param name="operatorId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ServiceResult<bool> ExecuteAntiepidemicPlan(string planId, DateTime executeDate, string effect, string principalId, string operatorId, string remark);

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
        ServiceResult<string> AddTreatment(string sheepId, string symptom, DateTime startDate, string disease, string treatmentPlan, int treatmentDays, string effect, string principalId, string operatorId, string remark);
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
        ServiceResult<bool> UpdateTreatment(string symptom, DateTime startDate, string disease, string treatmentPlan, int treatmentDays, string effect, string principalId, string remark, string id);

        #endregion

        #region Common
        ServiceResult<bool> SendEmail(string subject, string body);
        #endregion

        /// <summary>
        /// 添加羊场信息
        /// </summary>
        /// <param name="name">羊场名称</param>
        /// <param name="operatingRange">羊场经营范围</param>
        /// <param name="qualifications">羊场资质</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        ServiceResult<string> AddSheepFieldInformation(string name, string operatingRange, string qualifications, string remark);

        #region 报表

        //按照月份统计羊只繁殖情况
        List<MultiplyReport> GetMultiplyReport(DateTime? startDate, DateTime? endDate);

        //按时间段查询羊场收入（全部收入、卖羊总收入、卖羊毛总收入、卖羊粪总收入）
        List<SellReport> GetSellReport(DateTime? startDate, DateTime? endDate);

        //按时间段查询洋场的支出（买饲料、买羊、买药品、买其他、员工工资、水费、电费）
        List<BuyReport> GetBuyReport(DateTime? startDate, DateTime? endDate);

        //按时间段查询羊场的收支情况
        List<FinanceReport> GetFinanceReport(DateTime? startDate, DateTime? endDate);

        //获取饲料月出入库与库存统计
        List<FeedReport> GetFeedInventoryReport(DateTime? startDate, DateTime? endDate);

        //羊只进食情况 报表
        List<FeedSheepReport> GetFeedSheepReport(string sheepId);

        #endregion      
    }
}

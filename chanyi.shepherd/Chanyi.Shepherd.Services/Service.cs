using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Net.Mail;
using System.Net;
using System.Configuration;

using Common.Logging;

using Chanyi.Shepherd.IDao;
using Chanyi.Shepherd.IServices;
using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.Filter;
using Chanyi.Shepherd.QueryModel.Model.BaseInfo;
using Chanyi.Shepherd.QueryModel.Filter.BaseInfo;
using Chanyi.Shepherd.QueryModel.Model.System;
using Chanyi.Shepherd.QueryModel.Filter.HR;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Model.Multiplying;
using Chanyi.Shepherd.QueryModel.Filter.Multiplying;
using Chanyi.Shepherd.QueryModel.BindingFilter;
using Chanyi.Shepherd.QueryModel.Model.HR;
using Chanyi.Shepherd.QueryModel.Model.Assist;
using Chanyi.Shepherd.QueryModel.Filter.Assist;
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

namespace Chanyi.Shepherd.Services
{
    public class Service : IService
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public IDal Dao { get; set; }
        string feedNameCategory = ConfigurationManager.AppSettings["feedNameCategory"];
        string medicineNameCategory = ConfigurationManager.AppSettings["medicineNameCategory"];

        string defaultEmployee = ConfigurationManager.AppSettings["defaultEmployee"];

        #region Bind

        public ServiceResult<bool> Initialize()
        {
            try
            {
                Dao.Initialize();
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "0001", ex.Message);
            }
        }

        public List<SheepBind> GetSheepBind(SheepBindFilter filter)
        {
            return Dao.GetSheepBind(filter);
        }

        public List<SheepBind> GetSheepParentBind(string childId)
        {
            if (string.IsNullOrEmpty(childId))
                return Dao.GetSheepParentBind().Distinct().OrderBy(t => t.SerialNumber).ToList();
            return Dao.GetSheepParentBind(childId).Distinct().OrderBy(t => t.SerialNumber).ToList();
        }

        public List<SheepBind> GetAbortionSheepBind()
        {
            var pms = Dao.GetSheepParameterValue(SettingsEnum.PreDeliveryRemaindful).FirstOrDefault();
            return Dao.GetAbortionSheepBind(int.Parse(pms.Value), pms.Range);
        }

        public List<SheepBind> GetDeliverySheepBind()
        {
            var pms = Dao.GetSheepParameterValue(SettingsEnum.PreDeliveryRemaindful).FirstOrDefault();
            return Dao.GetDeliverySheepBind(int.Parse(pms.Value), pms.Range);
        }

        public List<SheepBind> GetMatingSheepSelectBind()
        {
            return Dao.GetMatingSheepSelectBind();
        }

        public List<SheepBind> GetMatingSheepCountBind()
        {
            return Dao.GetMatingSheepCountBind().Distinct().ToList();
        }

        public List<SheepBind> GetStudSheepBind()
        {
            return Dao.GetStudSheepBind();
        }

        public List<SheepBind> GetStudSheepBindWithOuter()
        {
            return Dao.GetStudSheepBindWithOuter();
        }

        public List<SheepBind> GetAbortionSheepSelectBind()
        {
            return Dao.GetAbortionSheepSelectBind();
        }

        public List<SheepBind> GetDeliverySheepSelectBind()
        {
            return Dao.GetDeliverySheepSelectBind();
        }

        public List<SheepBind> GetDeathSheepSelectBind()
        {
            return Dao.GetDeathSheepSelectBind();
        }

        public List<SheepBind> GetFirstAssessSheepSelectBind()
        {
            return Dao.GetFirstAssessSheepSelectBind();
        }

        public List<SheepBind> GetAssessStudSheepSelectBind()
        {
            return Dao.GetStudAssessSheepSelectBind();
        }
        public List<SheepBind> GetSecondAssessSheepAddBind()
        {
            return Dao.GetSecondAssessSheepAddBind();
        }

        public List<SheepBind> GetSecondAssessSheepSelectBind()
        {
            return Dao.GetSecondAssessSheepSelectBind();
        }

        public List<SheepBind> GetThirdAssessSheepAddBind()
        {
            return Dao.GetThirdAssessSheepAddBind();
        }

        public List<SheepBind> GetThirdAssessSheepSelectBind()
        {
            return Dao.GetThirdAssessSheepSelectBind();
        }

        public List<SheepBind> GetExceptAssessSheepAddBind()
        {
            return Dao.GetExceptAssessSheepAddBind();
        }
        public List<SheepBind> GetExceptAssessSheepSelectBind()
        {
            return Dao.GetExceptAssessSheepSelectBind();
        }

        public List<SheepBind> GetTreatmentSheepBind()
        {
            return Dao.GetTreatmentSheepBind();
        }

        public List<BreedBind> GetBreedBind()
        {
            return Dao.GetBreedBind();
        }

        public List<SheepfoldBind> GetSheepfoldBind()
        {
            return Dao.GetSheepfoldBind();
        }

        public List<SheepfoldSheepCountBind> GetSheepfoldSheepCountBind()
        {
            return Dao.GetSheepfoldSheepCountBind();
        }

        public List<EmployeeBind> GetEmployeeBind()
        {
            return Dao.GetEmployeeBind().Where(t => !t.Name.Equals(defaultEmployee)).ToList();
        }
        public List<EmployeeBind> GetEmployeeBindWithDefault()
        {
            return Dao.GetEmployeeBind();
        }
        public List<EmployeeBind> GetAllEmployeeBind()
        {
            return Dao.GetAllEmployeeBind();
        }


        public List<UserBind> GetUserBind()
        {
            return Dao.GetUserBind();
        }

        public List<DutyBind> GetDutyBind()
        {
            return Dao.GetDutyBind();
        }

        public Dictionary<SheepfoldBind, List<SheepBind>> GetMoveSheepfoldBind()
        {
            return Dao.GetMoveSheepfoldBind();
        }

        public Dictionary<DiseaseTypeBind, List<DiseaseBind>> GetDiseaseTypeBind()
        {
            return Dao.GetDiseaseTypeBind();
        }

        public List<DiseaseBind> GetDiseaseBindBySymptomIds(params string[] symptomIds)
        {
            return Dao.GetDiseaseBindBySymptomIds(symptomIds);
        }

        public List<DiseaseBind> GetDiseaseBindByName(string diseaseName)
        {
            return Dao.GetDiseaseBindByName(diseaseName);
        }

        public Dictionary<SymptomTypeBind, List<SymptomBind>> GetSymptomTypeBind()
        {
            return Dao.GetSymptomTypeBind();
        }

        public List<SymptomBind> GetSymptomBind(SymptomBindFilter filter)
        {
            return Dao.GetSymptomBind(filter);
        }

        public List<PurchaserBind> GetPurchaserBind()
        {
            return Dao.GetPurchaserBind();
        }

        public List<FeedNameBind> GetFeedNameBind()
        {
            return Dao.GetFeedNameBind();
        }
        public List<FeedTypeBind> GetFeedTypeBind()
        {
            return Dao.GetFeedTypeBind();
        }
        public List<FeedTypeBind> GetFeedTypeBind(string feedNameId)
        {
            return Dao.GetFeedTypeBind(feedNameId);
        }
        public List<AreaBind> GetAreaBind()
        {
            return Dao.GetAreaBind();
        }
        public List<AreaBind> GetAreaBind(string feedNameId, string typeId)
        {
            return Dao.GetAreaBind(feedNameId, typeId);
        }

        public List<MedicineBind> GetMedicineNameBind()
        {
            return Dao.GetMedicineNameBind();
        }
        public List<ManufactureBind> GetManufactureBind()
        {
            return Dao.GetManufactureBind();
        }
        public List<ManufactureBind> GetManufactureBind(string medicineNameId)
        {
            return Dao.GetManufactureBind(medicineNameId);
        }
        public List<DepartmentBind> GetDepartmentBind()
        {
            return Dao.GetDepartmentBind();
        }
        public List<DepartmentBind> GetDepartmentBind(string medicineNameId)
        {
            return Dao.GetDepartmentBind(medicineNameId);
        }
        public List<MedicineTypeBind> GetMedicineTypeBind()
        {
            return Dao.GetMedicineTypeBind();
        }
        public List<MedicineTypeBind> GetMedicineTypeBind(string MedicineNameId)
        {
            return Dao.GetMedicineTypeBind(MedicineNameId);
        }
        public List<CooperaterBind> GetManufacturerBind(string MedicineNameId, string typeId)
        {
            return Dao.GetManufacturerBind(MedicineNameId, typeId);
        }

        public List<OtherBind> GetOtherBind()
        {
            return Dao.GetOtherBind();
        }

        public List<SheepBind> GetBuySheepBind4Add()
        {
            return Dao.GetBuySheepBind4Add();
        }
        public List<SheepBind> GetBuySheepBind4Select()
        {
            return Dao.GetBuySheepBind4Select();
        }

        public List<BuyFeedBind> GetBuyFeedBind4Add()
        {
            return Dao.GetBuyFeedBind4Add();
        }
        public List<BuyMedicineBind> GetBuyMedicineBind4Add()
        {
            return Dao.GetBuyMedicineBind4Add();
        }
        public List<BuyOtherBind> GetBuyOtherBind4Add()
        {
            return Dao.GetBuyOtherBind4Add();
        }

        public List<SellFeedBind> GetSellFeedBind4Add()
        {
            return Dao.GetSellFeedBind4Add();
        }
        public List<SellOtherBind> GetSellOtherBind4Add()
        {
            return Dao.GetSellOtherBind4Add();
        }

        public List<FormulaBind> GetFormulaBind(bool? isEnable = true)
        {
            return Dao.GetFormulaBind((bool)isEnable);
        }

        #endregion

        #region Chart

        public List<PeriodsSheepGrowthStageCount> GetPeriodsSheepGrowthStageCount(DateTime date, string breedId, GenderEnum? gender = null)
        {
            return Dao.GetPeriodsSheepGrowthStageCount(date, breedId, gender);
        }

        public List<PeriodsSellSheepCount> GetPeriodsSellSheepCount(DateTime? dtStart = null, DateTime? dtEnd = null)
        {
            if (dtStart == null)
                dtStart = DateTime.Parse(DateTime.Today.Year + "-1-1");
            if (dtEnd == null)
                dtEnd = DateTime.Today;

            return Dao.GetPeriodsSellSheepCount((DateTime)dtStart, (DateTime)dtEnd);
        }

        #endregion

        #region BaseInfo

        public List<Sheep> GetSheep(SheepFilter filter, int rowsCount)
        {
            return Dao.GetSheep(filter, rowsCount);
        }

        public List<Sheep> GetSheep(SheepFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetSheep(filter, pageIndex, pageSize, out totalCount);
        }


        public List<Sheep> GetAllSheep(SheepFilter filter, int pageIndex, int pageSize, out int totalCounts)
        {
            return Dao.GetAllSheep(filter, pageIndex, pageSize, out totalCounts);
        }

        public Sheep GetSheepById(string id)
        {
            return Dao.GetSheepById(id);
        }

        public List<Breed> GetBreed(BreedFilter filter)
        {
            return Dao.GetBreed(filter);
        }
        public List<Breed> GetBreed(BreedFilter filter, int rowsCount)
        {
            return Dao.GetBreed(filter, rowsCount);
        }

        public List<Sheepfold> GetSheepfold(SheepfoldFilter filter)
        {
            return Dao.GetSheepfold(filter);
        }
        public List<Sheepfold> GetSheepfold(SheepfoldFilter filter, int rowsCount)
        {
            return Dao.GetSheepfold(filter, rowsCount);
        }

        public ServiceResult<string> AddSheep(string serialNumber, string breedId, GenderEnum gender, GrowthStageEnum growthStage, OriginEnum origin, float? birthWeight, int compatriotNumber, DateTime? birthDay, string fatherId, string motherId, string sheepfoldId, string principalId, string operatorId, string remark)
        {
            try
            {
                ServiceResult<bool> validateResult = ValidateSheepParents(fatherId, motherId, false);
                if (!validateResult.Result)
                {
                    return new ServiceResult<string>(null, ResultStatus.WANING, validateResult.Code, validateResult.Message);
                }

                int sheepCount = Dao.GetSheepCountBySerialNum(serialNumber);
                if (sheepCount > 0)
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "存在相同的羊只编号");

                string id = Guid.NewGuid().ToString();
                Dao.AddSheep(id, serialNumber, breedId, gender, growthStage, origin, birthWeight, compatriotNumber, birthDay, fatherId, motherId, sheepfoldId, SheepStatusEnum.Nomal, principalId, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        private bool existF, existM;
        ServiceResult<bool> ValidateSheepParents(string fatherId, string motherId, bool isPurchased)
        {
            try
            {
                existF = true;
                existM = true;
                if (!string.IsNullOrEmpty(fatherId) && !string.IsNullOrEmpty(motherId))
                {
                    if (fatherId.ToUpper().Equals(motherId.ToUpper()))
                        return new ServiceResult<bool>(false, ResultStatus.WANING, "2", "父编号与母编号相同");

                    List<Sheep> list = Dao.GetTwoSheeps(fatherId, motherId);
                    existF = list.Where(t => t.Id.ToUpper().Equals(fatherId.ToUpper())).Count() > 0;
                    existM = list.Where(t => t.Id.ToUpper().Equals(motherId.ToUpper())).Count() > 0;

                    if (!isPurchased)
                    {
                        //自繁
                        if (!existF)
                            return new ServiceResult<bool>(false, ResultStatus.WANING, "2", "父编号羊只不存在");
                        if (!existM)
                            return new ServiceResult<bool>(false, ResultStatus.WANING, "2", "母编号羊只不存在");
                    }
                }
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        /// <summary>
        /// 添加外部羊只(如购入羊只的父母，此羊只不在厂中)
        /// </summary>
        ServiceResult<bool> AddOuterSheep(string sheepId, string serialNumber, string breedId, GenderEnum gender, string sheepfoldId, string principalId, string operatorId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sheepId) || string.IsNullOrWhiteSpace(serialNumber))
                    return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", "父母ID或编号不合法");

                Dao.AddSheep(sheepId, serialNumber, breedId, gender, GrowthStageEnum.StudSheep, OriginEnum.Purchase, null, 0, null, null, null, sheepfoldId, SheepStatusEnum.Outer, principalId, operatorId, DateTime.Now, null);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<string> AddSheep(string serialNumber, string breedId, GenderEnum gender, GrowthStageEnum growthStage, OriginEnum origin, float? birthWeight, int compatriotNumber, DateTime? birthDay, string fatherId, string fatherSerialNumber, string motherId, string motherSerialNumber, string sheepfoldId, string principalId, string operatorId, string remark, string buySource, decimal buyMoney, float? buyWeight, DateTime buyOperationDate, string buyPrincipalId, string buyRemark)
        {
            try
            {
                ServiceResult<bool> validateResult = ValidateSheepParents(fatherId, motherId, true);

                if (!existF)
                {
                    var result = AddOuterSheep(fatherId, fatherSerialNumber, breedId, GenderEnum.Male, sheepfoldId, principalId, operatorId);
                    if (!result.Result)
                        return new ServiceResult<string>(null, ResultStatus.ERROR, "1", "父本羊只添加失败");
                }
                if (!existM)
                {
                    var result = AddOuterSheep(motherId, motherSerialNumber, breedId, GenderEnum.Female, sheepfoldId, principalId, operatorId);
                    if (!result.Result)
                        return new ServiceResult<string>(null, ResultStatus.ERROR, "1", "母本羊只添加失败");
                }

                if (!validateResult.Result)
                    return new ServiceResult<string>(null, ResultStatus.WANING, validateResult.Code, validateResult.Message);

                int sheepCount = Dao.GetSheepCountBySerialNum(serialNumber);
                if (sheepCount > 0)
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "存在相同的羊只编号");

                string id = Guid.NewGuid().ToString();
                Dao.AddSheep(id, serialNumber, breedId, gender, growthStage, origin, birthWeight, compatriotNumber, birthDay, fatherId, motherId, sheepfoldId, SheepStatusEnum.Nomal, principalId, operatorId, DateTime.Now, remark, buySource, buyMoney, buyWeight, buyOperationDate, buyPrincipalId, buyRemark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public int GetCurMonthBirthCount()
        {
            return Dao.GetCurMonthBirthCount();
        }


        public ServiceResult<bool> UpdateSheep(string breedId, GenderEnum gender, GrowthStageEnum growthStage, OriginEnum origin, float? birthWeight, int compatriotNumber, DateTime? birthDay, float? ablactationWeight, DateTime? ablactationDate, string fatherId, string motherId, string sheepfoldId, string principalId, string remark, string id)
        {
            try
            {
                Dao.UpdateSheep(breedId, gender, growthStage, origin, birthWeight, compatriotNumber, birthDay, ablactationWeight, ablactationDate, fatherId, motherId, sheepfoldId, SheepStatusEnum.Nomal, principalId, remark, id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<string> AddBreed(string name, string description, string operatorId, string remark)
        {
            try
            {
                if (Dao.GetBreedCountByName(name) > 0)
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "存在相同的羊只品种");

                string id = Guid.NewGuid().ToString();
                Dao.AddBreed(id, name, description, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<string> AddSheepFold(string name, string administrator, string operatorId, string remark)
        {
            try
            {
                if (Dao.GetSheepFoldCountByName(name) > 0)
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "存在相同的羊圈名称");

                string id = Guid.NewGuid().ToString();
                Dao.AddSheepFold(id, name, administrator, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }
        public Sheepfold GetSheepFoldById(string id)
        {
            return Dao.GetSheepFoldById(id);
        }
        public ServiceResult<bool> UpdateSheepFold(string name, string administrator, string remark, string id)
        {
            try
            {
                Dao.UpdateSheepFold(name, administrator, remark, id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public Farm GetFarm()
        {
            return Dao.GetFarm();
        }

        public ServiceResult<bool> EditFarm(string name, string contacts, string phone, string address, string code, string businessScope, string qualifications, string remark)
        {
            try
            {
                Farm model = Dao.GetFarm();
                if (model == null)
                {
                    string id = Guid.NewGuid().ToString();
                    Dao.AddFarm(id, name, contacts, phone, address, code, businessScope, qualifications, remark);
                    return new ServiceResult<bool>(true);
                }

                Dao.UpdateFarm(name, contacts, phone, address, code, businessScope, qualifications, remark, model.Id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        #endregion

        #region System

        public ServiceResult<string> Login(string userName, string password)
        {
            List<Login> list = Dao.GetLoginMsg(userName);
            if (list.Count != 1)
                return new ServiceResult<string>(string.Empty, ResultStatus.WANING, "03", "用户名或密码错误");

            Login model = list.FirstOrDefault();
            if (!model.IsEnabled)
                return new ServiceResult<string>(string.Empty, ResultStatus.WANING, "04", "用户名不可用");

            DateTime lastErrorTime = model.LastErrorTime ?? DateTime.Now;
            model.ErrorTimes = model.ErrorTimes ?? 0;

            double spanMinutes = (lastErrorTime.AddMinutes(120) - DateTime.Now).TotalMinutes;
            if (model.ErrorTimes >= 5 && spanMinutes >= 0)
                return new ServiceResult<string>(string.Empty, ResultStatus.WANING, "05", string.Format("账号被锁定，请{0}分钟后再试", Math.Ceiling(spanMinutes)));

            ServiceResult<string> result = new ServiceResult<string>(Dao.Login(userName, password));
            int errorTimes = 0;
            if (string.IsNullOrWhiteSpace(result.Result))
            {
                errorTimes = model.ErrorTimes >= 5 ? 1 : (int)model.ErrorTimes + 1;
                result = new ServiceResult<string>(string.Empty, ResultStatus.WANING, "06", "用户名或密码错误（" + errorTimes + "次）");
                lastErrorTime = DateTime.Now;
            }

            Dao.UpdateLoginErrorTimes(userName, errorTimes, lastErrorTime);
            return result;
        }

        public ServiceResult<bool> UpdatePassword(string oldPassword, string newPassword, string id)
        {
            try
            {
                if (Dao.GetUserCount(oldPassword, id) != 1)
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "2", "原始密码错误");

                Dao.UpdatePassword(newPassword, id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<bool> UpdatePassword(string password, string id)
        {
            try
            {
                Dao.UpdatePassword(password, id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<SheepParameter> GetSheepParameters()
        {
            return Dao.GetSheepParameters();
        }

        public ServiceResult<bool> UpdateSheepParameters(List<UpdateSheepParameter> list, int deliveryRangeDays)
        {
            try
            {
                Dao.UpdateSheepParameters(list, deliveryRangeDays);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<FeedCritical> GetFeedCritical()
        {
            return Dao.GetFeedCritical();
        }
        public List<MedicineCritical> GetMedicineCritical()
        {
            return Dao.GetMedicineCritical();
        }

        public ServiceResult<bool> UpdateCritical(List<UpdateCritical> list)
        {
            try
            {
                //获取已经存在的临界值Id集合
                //List<string> existIds = Dao.GetCriticalIds(list.Select(t => t.KindId).ToList()).Select(l => l.Id).ToList();

                List<UpdateCritical> listUpdate = new List<UpdateCritical>();
                List<UpdateCritical> listAdd = new List<UpdateCritical>();
                list.ForEach(l =>
                {
                    //if (existIds.Contains(l.Id))
                    if (string.IsNullOrEmpty(l.Id))
                        listAdd.Add(l);
                    else
                        listUpdate.Add(l);
                });

                Dao.UpdateCritical(listUpdate);
                Dao.AddCritical(listAdd);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<PreDeliveryRemaindful> GetPreDeliveryRemaindful()
        {
            SheepParameter pm = Dao.GetSheepParameterValue(SettingsEnum.PreDeliveryRemaindful).FirstOrDefault();
            if (!pm.IsRemaindful)
                return new List<PreDeliveryRemaindful>();
            return Dao.GetPreDeliveryRemaindful(pm);
        }

        public List<PreAblactationRemaindful> GetPreAblactationRemaindful()
        {
            SheepParameter pm = Dao.GetSheepParameterValue(SettingsEnum.PreAblactationRemaindful).FirstOrDefault();
            if (!pm.IsRemaindful)
                return new List<PreAblactationRemaindful>();
            return Dao.GetPreAblactationRemaindful(Convert.ToInt32(pm.Value));
        }

        public List<FeedRemaindful> GetFeedRemaindful()
        {
            return Dao.GetFeedRemaindful();
        }

        public List<MedicineRemaindful> GetMedicineRemaindful()
        {
            return Dao.GetMedicineRemaindful();
        }

        public List<string> GetPreDeliverySerialNumber()
        {
            SheepParameter pm = Dao.GetSheepParameterValue(SettingsEnum.PreDeliveryRemaindful).FirstOrDefault();
            if (pm == null || !pm.IsRemaindful)
                return new List<string>();
            return Dao.GetPreDeliverySerialNumber(pm).Select(t => t.SerialNumber).ToList();
        }

        public List<string> GetPreAblactationSerialNumber()
        {

            SheepParameter pm = Dao.GetSheepParameterValue(SettingsEnum.PreAblactationRemaindful).FirstOrDefault();
            if (pm == null || !pm.IsRemaindful)
                return new List<string>();
            return Dao.GetPreAblactationSerialNumber(Convert.ToInt32(pm.Value)).Select(t => t.SerialNumber).ToList();
        }

        public List<Permission> GetAllPermissions(PermissionFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetAllPermissions(filter, pageIndex, pageSize, out totalCount);
        }

        public List<Role> GetAllRoles(RoleFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetAllRoles(filter, pageIndex, pageSize, out totalCount);
        }

        public List<Permission> GetPermissionsByRoleId(string roleId)
        {
            return Dao.GetPermissionsByRoleId(roleId);
        }

        public List<Permission> GetAllPermissionByUserId(string userId)
        {
            return Dao.GetAllPermissionByUserId(userId);
        }

        public List<Permission> GetPermissionByUserId(string userId)
        {
            return Dao.GetPermissionByUserId(userId);
        }

        public List<Role> GetAllRoleByUserId(string userId)
        {
            return Dao.GetAllRoleByUserId(userId);
        }

        public List<Permission> GetRoleAvailablePermissions(string keyWord, string roleId)
        {
            return Dao.GetRoleAvailablePermissions(keyWord, roleId);
        }

        public List<Permission> GetUserAvailablePermissions(string keyWord, string userId)
        {
            return Dao.GetUserAvailablePermissions(keyWord, userId);
        }

        public List<Role> GetUserAvailableRoles(string keyWord, string userId)
        {
            return Dao.GetUserAvailableRoles(keyWord, userId);
        }

        public ServiceResult<string> AddUser(string userName, string password, string operatorId, string remark)
        {
            try
            {
                //验证重复用户名
                if (Dao.GetUserCount(userName) != 0)
                    return new ServiceResult<string>(string.Empty, ResultStatus.WANING, "2", "存在相同的用户名");

                string id = Guid.NewGuid().ToString();
                Dao.AddUser(id, userName, password, 0, DateTime.Now.AddDays(-1), operatorId, DateTime.Now, true, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<string> AddRole(string name, string description, string operatorId, string remark)
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                Dao.AddRole(id, name, description, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<string> AddPermission(string name, string description, string URL, string operatorId, string remark)
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                Dao.AddPermission(id, name, description, URL, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }


        public Role GetRoleById(string id)
        {
            return Dao.GetRoleById(id);
        }

        public Permission GetPermissionById(string id)
        {
            return Dao.GetPermissionById(id);
        }


        public ServiceResult<bool> UpdateRole(string name, string description, string remark, string id)
        {
            try
            {
                Dao.UpdateRole(name, description, remark, id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<bool> UpdatePermission(string name, string description, string remark, string id)
        {
            try
            {
                Dao.UpdatePermission(name, description, remark, id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<bool> DeletePermission(string id)
        {
            try
            {
                Dao.DeletePermission(id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<bool> DeleteRole(string id)
        {
            try
            {
                Dao.DeleteRole(id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }


        public ServiceResult<bool> GrantPermission2Role(List<string> permissionIds, string roleId)
        {
            try
            {
                //删除指定角色的所有已权限
                Dao.DeleteRolePermission(roleId);

                Dao.GrantPermission2Role(permissionIds, roleId);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<bool> GrantRole2User(List<string> roleIds, string userId)
        {
            try
            {
                //删除指定用户的所有角色
                Dao.DeleteUserRole(userId);

                Dao.GrantRole2User(roleIds, userId);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<bool> GrantPermission2User(List<string> permissionIds, string userId)
        {
            try
            {
                //删除指定用户的所有权限
                Dao.DeleteUserPermission(userId);

                Dao.GrantPermission2User(permissionIds, userId);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }


        public ServiceResult<string> AddSplitYard(string name, string manager)
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                Dao.AddSplitYard(id, name, manager);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }


        public List<SplitYard> GetAllSplitYard(SplitYardFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetAllSplitYard(filter, pageIndex, pageSize, out totalCount);
        }

        public SplitYard GetSplitYardById(string id)
        {
            return Dao.GetSplitYardById(id);
        }


        public ServiceResult<bool> UpdateSplitYard(string name, string manager, string id)
        {
            try
            {
                Dao.UpdateSplitYard(name, manager, id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        #endregion

        #region Multiplying

        public ServiceResult<string> AddMating(string femaleId, string maleId, DateTime matingDate, string principalId, string operatorId, string remark)
        {
            try
            {
                //验证两只羊能不能配种
                if (maleId.ToUpper().Equals(femaleId.ToUpper()))
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "相同羊只编号无法进行配种");

                List<Sheep> list = Dao.GetTwoSheeps(maleId, femaleId);

                if (list.Where(t => t.Id.ToUpper().Equals(maleId.ToUpper())).Count() <= 0)
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "公羊编号不存在，或者不是种公羊");

                if (list.Where(t => t.Id.ToUpper().Equals(femaleId.ToUpper())).Count() <= 0)
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "母羊编号不存在，或者不是种母羊");

                if (list.Select(s => s.Gender).Distinct().Count() <= 1)
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "要配种的羊只性别不能相同");


                //将之前所有的交配记录是否提醒设置为false
                ServiceResult<bool> result = UpdateMatingDisRemindful(maleId);
                if (!result.Result)
                    return new ServiceResult<string>(null, ResultStatus.ERROR, "03", result.Message);

                string id = Guid.NewGuid().ToString();
                Dao.AddMating(id, femaleId, maleId, matingDate, true, principalId, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<bool> UpdateMatingDisRemindful(string maleId)
        {
            try
            {
                Dao.UpdateMatingDisRemindful(maleId);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<Mating> GetMating(MatingFilter filter, int pageIndex, int pageSize, out int totalCount)
        {

            return Dao.GetMating(filter, pageIndex, pageSize, out totalCount);
        }

        public List<Mating> GetMating(MatingFilter filter, int rowsCount)
        {
            return Dao.GetMating(filter, rowsCount);
        }

        public Mating GetMatingById(string id)
        {
            return Dao.GetMatingById(id);
        }

        public ServiceResult<bool> DeleteMating(string id)
        {
            try
            {
                Mating model = Dao.GetMatingById(id);
                if (model == null)
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "2", "死亡信息不存在");
                if (model.IsDel)
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "2", "死亡信息已经删除");


                Dao.DeleteMating(id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<MatingCount> GetMatingCount(string sheepId, int? count, int? year, SeasonEnum? season, int pageIndex, int pageSize, out int totalCount)
        {
            DateTime?[] dates = GetMatingStartEnd(year, season);
            return Dao.GetMatingCount(sheepId, count, dates[0], dates[1], pageIndex, pageSize, out totalCount);
        }
        DateTime?[] GetMatingStartEnd(int? year, SeasonEnum? season)
        {
            DateTime?[] dates = { null, null };
            if (year != null)
            {
                string s = "-1-1";
                string e = "-12-31";
                if (season != null)
                {
                    string str = ConfigurationManager.AppSettings[season.ToString()];
                    if (!string.IsNullOrEmpty(str) && str.Contains(","))
                    {
                        string[] strArr = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        s = "-" + strArr[0];
                        e = "-" + strArr[1];
                    }
                }

                try
                {
                    dates[0] = DateTime.Parse(year + s);
                    dates[1] = DateTime.Parse(year + e);
                }
                catch (Exception)
                {
                    dates[0] = DateTime.Parse(year + "-1-1");
                    dates[1] = DateTime.Parse(year + "-12-31");
                    //throw;
                }
            }
            return dates;
        }

        public List<MatingCount> GetMatingCount(string sheepId, int? count, int? year, SeasonEnum? season, int rowsCount)
        {
            DateTime?[] dates = GetMatingStartEnd(year, season);
            return Dao.GetMatingCount(sheepId, count, dates[0], dates[1], rowsCount);
        }

        public ServiceResult<string> AddAbortion(string sheepId, string reason, string dispose, DateTime abortionDate, string principalId, string operatorId, string remark)
        {
            try
            {
                var result = IsSheepCanAborationOrDelivery(sheepId, abortionDate, "流产");
                if (string.IsNullOrEmpty(result.Result))
                    return result;

                string id = Guid.NewGuid().ToString();
                Dao.AddAbortion(id, sheepId, reason, dispose, abortionDate, principalId, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }
        ServiceResult<string> IsSheepCanAborationOrDelivery(string sheepId, DateTime date, string msg)
        {
            //最长允许分娩时长
            //object longestDeliveryDays = ConfigurationManager.AppSettings["longestDeliveryDays"];
            //double preDelivery = longestDeliveryDays == null ? 150 : Convert.ToInt32(longestDeliveryDays);

            Sheep existSheep = Dao.GetSheepById(sheepId);
            if (existSheep == null)
                return new ServiceResult<string>(null, ResultStatus.WANING, "1", "母羊羊只不存在");
            //交配记录
            Mating mating = Dao.GetLatestMatingByFemaleId(sheepId);

            //if (mating == null || (DateTime.Now - mating.MatingDate).TotalDays > preDelivery || mating.MatingDate > date)
            //    return new ServiceResult<string>(null, ResultStatus.WANING, "1", "该羊只近期没有交配");

            //流产记录(大于交配时间)
            object latestAbortionDate = Dao.GetLatestAbortionDateBySheepId(sheepId);
            if (latestAbortionDate != null && !string.IsNullOrWhiteSpace(latestAbortionDate.ToString()) && DateTime.Parse(latestAbortionDate.ToString()) > mating.MatingDate)
                return new ServiceResult<string>(null, ResultStatus.WANING, "1", "该羊只已经" + msg + "，请重新交配");
            //分娩记录
            object latestDeliveryDate = Dao.GetlatestDeliveryDateBySheepId(sheepId);
            if (latestDeliveryDate != null && !string.IsNullOrWhiteSpace(latestDeliveryDate.ToString()) && DateTime.Parse(latestDeliveryDate.ToString()) > mating.MatingDate)
                return new ServiceResult<string>(null, ResultStatus.WANING, "1", "该羊只已经" + msg + "，请重新交配");

            return new ServiceResult<string>("yes");
        }
        public ServiceResult<string> AddDelivery(string femaleId, DeliveryWayEnum deliveryWay, MidwiferyReasonEnum deliverReason, string deliverReasonOtherDetail, int? liveMaleCount, int? liveFemaleCount, int totalCount, DateTime deliveryDate, string principalId, string operatorId, string remark, List<Chanyi.Shepherd.QueryModel.AddModel.BaseInfo.Sheep> lambList)
        {
            try
            {
                var result = IsSheepCanAborationOrDelivery(femaleId, deliveryDate, "分娩");
                if (string.IsNullOrEmpty(result.Result))
                    return result;

                string id = Guid.NewGuid().ToString();

                if (deliveryWay == DeliveryWayEnum.Normal)
                {
                    deliverReason = MidwiferyReasonEnum.None;
                    deliverReasonOtherDetail = null;
                }
                if (lambList.Count <= 0)
                {
                    Dao.AddDelivery(id, femaleId, deliveryWay, deliverReason, deliverReasonOtherDetail, liveMaleCount, liveFemaleCount, totalCount, deliveryDate, principalId, operatorId, DateTime.Now, remark);
                }
                else
                {
                    //获取父Id
                    List<Sheep> list = Dao.GetMatingBreedInfoByFemaleId(femaleId);
                    Sheep male = list.Where(m => m.Gender == GenderEnum.Male).FirstOrDefault();
                    Sheep female = list.Where(f => f.Gender == GenderEnum.Female).FirstOrDefault();
                    if (list.Count < 2 || male == null || female == null)
                        return new ServiceResult<string>(null, ResultStatus.WANING, "2", "交配信息不完整");

                    string lambBreedId;
                    string fatherId = male.Id;
                    //获取品种
                    if (list[0].BreedId.ToUpper() == list[1].BreedId.ToUpper())
                        lambBreedId = male.BreedId;
                    else
                    {
                        string newBreedName = male.BreedName.Substring(0, 1) + female.BreedName.Substring(0, 1);
                        Breed breed = Dao.GetBreedByName(newBreedName);
                        if (breed != null)
                        {
                            lambBreedId = breed.Id;
                        }
                        else
                        {
                            var addBreedResult = AddBreed(newBreedName, male.BreedName + female.BreedName, operatorId, null);
                            if (string.IsNullOrEmpty(addBreedResult.Result))
                                return new ServiceResult<string>(null, ResultStatus.WANING, "2", "添加新品种出错");
                            lambBreedId = addBreedResult.Result;
                        }
                    }

                    Dao.AddDelivery(id, femaleId, deliveryWay, deliverReason, deliverReasonOtherDetail, liveMaleCount, liveFemaleCount, totalCount, deliveryDate, principalId, operatorId, DateTime.Now, remark, lambList, lambBreedId, fatherId);
                }

                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<string> AddAblactation(string sheepId, float ablactationWeight, DateTime ablactationDate, string principalId, string operatorId, string remark)
        {
            try
            {
                if (Dao.GetAblactation(sheepId) > 0)
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "这只羊已经登记");

                string id = Guid.NewGuid().ToString();
                Dao.AddAblactation(id, sheepId, ablactationWeight, ablactationDate, principalId, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<Delivery> GetDelivery(DeliveryFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetDelivery(filter, pageIndex, pageSize, out totalCount);
        }

        public List<Delivery> GetDelivery(DeliveryFilter filter, int rowsCount)
        {
            return Dao.GetDelivery(filter, rowsCount);
        }

        public Delivery GetDeliveryById(string id)
        {
            return Dao.GetDeliveryById(id);
        }
        public ServiceResult<bool> DeleteDelivery(string id)
        {
            try
            {
                Delivery model = Dao.GetDeliveryById(id);
                if (model == null)
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "2", "死亡信息不存在");
                if (model.IsDel)
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "2", "死亡信息已经删除");

                Dao.DeleteDelivery(id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<Abortion> GetAbortion(AbortionFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetAboration(filter, pageIndex, pageSize, out totalCount);
        }

        public List<Abortion> GetAbortion(AbortionFilter filter, int rowsCount)
        {
            return Dao.GetAboration(filter, rowsCount);
        }
        public Abortion GetAbortionById(string id)
        {
            return Dao.GetAbortionById(id);
        }
        public ServiceResult<bool> DeleteAbortion(string id)
        {
            try
            {
                Abortion model = Dao.GetAbortionById(id);
                if (model == null)
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "2", "死亡信息不存在");
                if (model.IsDel)
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "2", "死亡信息已经删除");

                Dao.DeleteAbortion(id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<Ablactation> GetAblactation(AblactationFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetAblactation(filter, pageIndex, pageSize, out totalCount);
        }
        public List<Ablactation> GetAblactation(AblactationFilter filter, int rowsCount)
        {
            return Dao.GetAblactation(filter, rowsCount);
        }
        public Ablactation GetAblactationById(string id)
        {
            return Dao.GetAblactationById(id);
        }

        public ServiceResult<bool> UpdateMating(string femaleId, string maleId, DateTime matingDate, string principalId, string remark, string id)
        {
            try
            {
                Dao.UpdateMating(femaleId, maleId, matingDate, true, principalId, remark, id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<bool> UpdateDelivery(DeliveryWayEnum deliveryWay, MidwiferyReasonEnum deliverReason, string deliverReasonOtherDetail, int? liveMaleCount, int? liveFemaleCount, int totalCount, DateTime deliveryDate, string principalId, string remark, string id)
        {
            try
            {
                Dao.UpdateDelivery(deliveryWay, deliverReason, deliverReasonOtherDetail, liveMaleCount, liveFemaleCount, totalCount, deliveryDate, principalId, remark, id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<bool> UpdateAbortion(string reason, string dispose, DateTime abortionDate, string principalId, string remark, string id)
        {
            try
            {
                Dao.UpdateAbortion(reason, dispose, abortionDate, principalId, remark, id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<bool> UpdateAblactation(float ablactationWeight, DateTime ablactationDate, string sheepId, string principalId, string remark, string id)
        {
            try
            {
                //顺便更新羊只
                Dao.UpdateAblactation(ablactationWeight, ablactationDate, sheepId, principalId, remark, id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        #endregion

        #region Breeding

        //TODO:以下四个通用
        //TODO:经过种羊鉴定之后，是否可以再次进行鉴定
        //TODO:如何进行计算，更新羊的成长阶段信息
        public ServiceResult<string> AddAssessStudsheep(string studSheepId, float matingAbility, float weight, float habitusScore, DateTime assessDate, string principalId, string operatorId, string remark)
        {
            try
            {
                if (Dao.GetStudsheepCount(studSheepId) > 0)
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "这只羊已经鉴定");

                string id = Guid.NewGuid().ToString();
                Dao.AddAssessStudsheep(id, studSheepId, matingAbility, weight, habitusScore, assessDate, principalId, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<string> AddFirstAssess(string sheepId, float weight, float habitusScore, DateTime assessDate, string principalId, string operatorId, string remark)
        {
            try
            {
                if (Dao.GetFirstAssessCount(sheepId) > 0)
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "这只羊已经鉴定");

                string id = Guid.NewGuid().ToString();
                Dao.AddFirstAssess(id, sheepId, weight, habitusScore, assessDate, principalId, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<string> AddSecondAssess(string sheepId, float breedFeatureScore, float genitaliaScore, float weight, float habitusScore, DateTime assessDate, string principalId, string operatorId, string remark)
        {
            try
            {
                if (Dao.GetSecondAssess(sheepId) > 0)
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "这只羊已经鉴定");

                string id = Guid.NewGuid().ToString();
                Dao.AddSecondAssess(id, sheepId, breedFeatureScore, genitaliaScore, weight, habitusScore, assessDate, principalId, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<string> AddThirdAssess(string sheepId, float matingAbility, float weight, float habitusScore, DateTime assessDate, string principalId, string operatorId, string remark)
        {
            try
            {
                if (Dao.GetThirdAssess(sheepId) > 0)
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "这只羊已经鉴定");

                string id = Guid.NewGuid().ToString();
                Dao.AddThirdAssess(id, sheepId, matingAbility, weight, habitusScore, assessDate, principalId, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<string> AddExceptAssessSheep(string sheepId, string reason, string principalId, string operatorId, string remark)
        {
            try
            {
                Sheep model = Dao.GetSheepById(sheepId);
                if (model == null)
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "羊只不存在");
                else if (model.GrowthStage == GrowthStageEnum.FattingSheep)
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "这只羊不需要否决");

                string id = Guid.NewGuid().ToString();
                Dao.AddExceptAssessSheep(id, sheepId, reason, model.GrowthStage, principalId, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<bool> DeleteExceptAssessSheep(string id)
        {
            try
            {
                Dao.DeleteExceptAssessSheep(id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<AssessStudsheep> GetAssessStudsheep(AssessStudsheepFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetAssessStudsheep(filter, pageIndex, pageSize, out totalCount);
        }
        public List<AssessStudsheep> GetAssessStudsheep(AssessStudsheepFilter filter, int rowCount)
        {
            return Dao.GetAssessStudsheep(filter, rowCount);
        }

        public AssessStudsheep GetAssessStudsheepById(string id)
        {
            return Dao.GetAssessStudsheepById(id);
        }

        public List<FirstAssess> GetFirstAssess(FirstAssessFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetFirstAssess(filter, pageIndex, pageSize, out totalCount);
        }
        public List<FirstAssess> GetFirstAssess(FirstAssessFilter filter, int rowCount)
        {
            return Dao.GetFirstAssess(filter, rowCount);
        }

        public FirstAssess GetFirstAssessById(string id)
        {
            return Dao.GetFirstAssessById(id);
        }


        public List<SecondAssess> GetSecondAssess(SecondAssessFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetSecondAssess(filter, pageIndex, pageSize, out totalCount);
        }

        public List<SecondAssess> GetSecondAssess(SecondAssessFilter filter, int rowCount)
        {
            return Dao.GetSecondAssess(filter, rowCount);
        }
        public SecondAssess GetSecondAssessById(string id)
        {
            return Dao.GetSecondAssessById(id);
        }

        public List<ThirdAssess> GetThirdAssess(ThirdAssessFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetThirdAssess(filter, pageIndex, pageSize, out totalCount);
        }
        public List<ThirdAssess> GetThirdAssess(ThirdAssessFilter filter, int rowCount)
        {
            return Dao.GetThirdAssess(filter, rowCount);
        }
        public ThirdAssess GetThirdAssessById(string id)
        {
            return Dao.GetThirdAssessById(id);
        }

        public List<ExceptAssess> GetExceptAssess(ExceptAssessFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            filter.IsDel = false;
            return Dao.GetExceptAssess(filter, pageIndex, pageSize, out totalCount);
        }
        public List<ExceptAssess> GetExceptAssess(ExceptAssessFilter filter, int rowCount)
        {
            filter.IsDel = false;
            return Dao.GetExceptAssess(filter, rowCount);
        }

        public ServiceResult<bool> UpdateAssessStudsheep(float matingAbility, float weight, float habitusScore, DateTime assessDate, string principalId, string remark, string id)
        {
            try
            {
                Dao.UpdateAssessStudsheep(matingAbility, weight, habitusScore, assessDate, principalId, remark, id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<bool> UpdateFirstAssess(float weight, float habitusScore, DateTime assessDate, string principalId, string remark, string id)
        {
            try
            {
                Dao.UpdateFirstAssess(weight, habitusScore, assessDate, principalId, remark, id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<bool> UpdateSecondAssess(float breedFeatureScore, float genitaliaScore, float weight, float habitusScore, DateTime assessDate, string principalId, string remark, string id)
        {
            try
            {
                Dao.UpdateSecondAssess(breedFeatureScore, genitaliaScore, weight, habitusScore, assessDate, principalId, remark, id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<bool> UpdateThirdAssess(float matingAbility, float weight, float habitusScore, DateTime assessDate, string principalId, string remark, string id)
        {
            try
            {
                Dao.UpdateThirdAssess(matingAbility, weight, habitusScore, assessDate, principalId, remark, id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        #endregion

        #region HR

        public ServiceResult<string> AddEmployee(string name, GenderEnum gender, string idNum, DateTime entryDate, decimal salary, string serialNum, string dutyId, EmployeeStatusEnum status, string principalId, string operatorId, string remark)
        {
            try
            {
                if (Dao.GetEmployeeCountByName(name) > 0)
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "存在相同的员工姓名");
                if (Dao.GetEmployeeCountBySerialNum(serialNum) > 0)
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "存在相同的员工编号");

                string id = Guid.NewGuid().ToString();
                Dao.AddEmployee(id, name, gender, idNum, entryDate, salary, serialNum, dutyId, status, principalId, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<Employee> GetEmployee(EmployeeFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetEmployee(filter, pageIndex, pageSize, out totalCount);
        }
        public List<Employee> GetEmployee(EmployeeFilter filter, int rowsCount)
        {
            return Dao.GetEmployee(filter, rowsCount);
        }
        public Employee GetEmployeeById(string id)
        {
            return Dao.GetEmployeeById(id);
        }

        public ServiceResult<bool> UpdateEmployee(string name, GenderEnum gender, string idNum, DateTime entryDate, decimal salary, string serialNum, string dutyId, EmployeeStatusEnum status, string principalId, string remark, string id)
        {
            try
            {
                if (Dao.GetEmployeeById(id).Name.ToUpper().Equals(defaultEmployee.ToUpper()))
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "2", "系统默认员工不能修改");

                Employee model1 = Dao.GetEmployeeByName(name);
                Employee model2 = Dao.GetEmployeeBySerialNum(serialNum);
                if (model1 != null && !model1.Id.ToUpper().Equals(id.ToUpper()))
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "2", "存在相同的员工姓名");
                if (model2 != null && !model2.Id.ToUpper().Equals(id.ToUpper()))
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "2", "存在相同的员工编号");

                Dao.UpdateEmployee(name, gender, idNum, entryDate, salary, serialNum, dutyId, status, principalId, remark, id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<string> AddQuit(string employeeId, string reason, DateTime quitDate, string principalId, string operatorId, string remark)
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                Dao.AddQuit(id, employeeId, reason, quitDate, principalId, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }
        public List<Quit> GetQuit(QuitFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetQuit(filter, pageIndex, pageSize, out totalCount);
        }
        public List<Quit> GetQuit(QuitFilter filter, int rowsCount)
        {
            return Dao.GetQuit(filter, rowsCount);
        }

        public ServiceResult<string> AddDuty(string name, string description, string operatorId, string remark)
        {
            try
            {
                if (Dao.GetDutyCountByName(name) > 0)
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "存在相同的职务名称");

                string id = Guid.NewGuid().ToString();
                Dao.AddDuty(id, name, description, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<User> GetUser(int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetUser(pageIndex, pageSize, out totalCount);
        }
        public List<User> GetUser(int rowsCount)
        {
            return Dao.GetUser(rowsCount);
        }
        public User GetUserById(string id)
        {
            return Dao.GetUserById(id);
        }

        public ServiceResult<bool> UpdateUser(bool isEnabled, string remark, string id)
        {
            try
            {
                Dao.UpdateUser(isEnabled, remark, id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<Cooperater> GetCooperater(CooperaterFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetCooperater(filter, pageIndex, pageSize, out totalCount);
        }

        public List<Cooperater> GetCooperater(CooperaterFilter filter, int rowsCount)
        {
            return Dao.GetCooperater(filter, rowsCount);
        }
        public Cooperater GetCooperaterById(string id)
        {
            return Dao.GetCooperaterById(id);
        }

        public ServiceResult<bool> UpdatePurchaser(string name, string department, string contactInfo, string principalId, string remark, string id)
        {
            try
            {
                Dao.UpdatePurchaser(name, department, contactInfo, principalId, remark, id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<string> AddPurchaser(string name, string department, string contactInfo, string operatorId, string principalId, string remark)
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                Dao.AddPurchaser(id, name, department, contactInfo, principalId, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        #endregion

        #region GroupManage

        public ServiceResult<bool> AddMoveSheepfold(IEnumerable<string> sheepIds, string targetSheepfold, string principalId, string operatorId, string remark)
        {
            try
            {
                List<Sheep> sheeps = Dao.GetSheepByIds(sheepIds);
                Dao.AddMoveSheepfold(sheeps, targetSheepfold, principalId, operatorId, DateTime.Now, remark);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<string> AddDeathManage(string sheepId, string reason, DeathDisposeEnum dispose, DateTime deathDate, string principalId, string operatorId, string remark)
        {
            try
            {
                DeathManage model = Dao.GetDeathManageById(sheepId);
                if (model != null && model.IsDel == true)
                    return new ServiceResult<string>(null, ResultStatus.WANING, "02", "该羊只死亡已登记");
                Sheep sheep = Dao.GetSheepById(sheepId);
                if (sheep != null && sheep.CreateTime > deathDate.AddDays(1))
                    return new ServiceResult<string>(null, ResultStatus.WANING, "02", "死亡时间不能早于进场时间");

                string id = Guid.NewGuid().ToString();
                string sysSheepfoldId = Dao.GetSysSheepfoldId(principalId, operatorId);
                Dao.AddDeathManage(id, sheepId, reason, dispose, deathDate, principalId, operatorId, DateTime.Now, remark, sysSheepfoldId);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<DeathManage> GetDeathManage(DeathManageFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetDeathManage(filter, pageIndex, pageSize, out totalCount);
        }

        public List<DeathManage> GetDeathManage(DeathManageFilter filter, int rowsCount)
        {
            return Dao.GetDeathManage(filter, rowsCount);
        }
        public DeathManage GetDeathManageById(string id)
        {
            return Dao.GetDeathManageById(id);
        }

        public ServiceResult<bool> DeleteDeathManage(string id)
        {
            try
            {
                DeathManage model = Dao.GetDeathManageById(id);
                if (model == null)
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "2", "死亡信息不存在");
                if (model.IsDel)
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "2", "死亡信息已经删除");

                Dao.DeleteDeathManage(id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<bool> UpdateDeathManage(string reason, DeathDisposeEnum dispose, DateTime deathDate, string principalId, string remark, string id)
        {
            try
            {
                Dao.UpdateDeathManage(reason, dispose, deathDate, principalId, remark, id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<MoveSheepfold> GetMoveSheepfold(MoveSheepfoldFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetMoveSheepfold(filter, pageIndex, pageSize, out totalCount);
        }
        public List<MoveSheepfold> GetMoveSheepfold(MoveSheepfoldFilter filter, int rowsCount)
        {
            return Dao.GetMoveSheepfold(filter, rowsCount);
        }

        #endregion

        #region Assist

        //public List<DiseaseType> GetDiseaseType(string pid = "0")
        //{
        //    return Dao.GetDiseaseType(pid);
        //}

        //public List<Disease> GetDiseaseByType(string typeId)
        //{
        //    return Dao.GetDiseaseByType(typeId);
        //}


        //public List<Disease> GetDiseaseBySymptomName(string symptomName)
        //{
        //    return Dao.GetDiseaseBySymptomName(symptomName);
        //}

        //public List<Disease> GetCrossDiseaseBySymptomIds(params string[] symptomIds)
        //{
        //    return Dao.GetCrossDiseaseBySymptomIds(symptomIds);
        //}

        #endregion

        #region Formula

        public List<Formula> GetFormula(FormulaFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetFormula(filter, pageIndex, pageSize, out totalCount);
        }
        public List<Formula> GetFormula(FormulaFilter filter, int rowsCount)
        {
            return Dao.GetFormula(filter, rowsCount);
        }
        public List<FormulaFeed> GetFormulaFeedById(string formulaId)
        {
            return Dao.GetFormulaFeedById(formulaId);
        }

        public List<SimpleFeed> GetSimpleFeed()
        {
            return Dao.GetSimpleFeed();
        }

        public List<FormulaNutrient> GetFormulaNutrient(FormulaNutrientFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetFormulaNutrient(filter, pageIndex, pageSize, out totalCount);
        }
        public List<FormulaNutrient> GetFormulaNutrient(FormulaNutrientFilter filter, int rowsCount)
        {
            return Dao.GetFormulaNutrient(filter, rowsCount);
        }
        public FormulaNutrient GetFormulaNutrientById(string id)
        {
            return Dao.GetFormulaNutrientById(id);
        }

        public ServiceResult<bool> UpdateFormulaStatus(bool isEnable, string id)
        {
            try
            {
                Dao.UpdateFormulaStatus(isEnable, id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<string> AddFormulaNutrient(string name, float? dailyGain, float? CP, float? DMI, float? EE, float? CF, float? NFE, float? Ash, float? NDF, float? ADF, float? Starch, float? Ga, float? AllP, float? Arg, float? His, float? Ile, float? Leu, float? Lys, float? Met, float? Cys, float? Phe, float? Tyr, float? Thr, float? Trp, float? Val, float? P, float? Na, float? Cl, float? Mg, float? K, float? Fe, float? Cu, float? Mn, float? Zn, float? Se, float? Carotene, float? VE, float? VB1, float? VB2, float? PantothenicAcid, float? Niacin, float? Biotin, float? Folic, float? Choline, float? VB6, float? VB12, float? LinoleicAcid, float? Salt, string principalId, string operatorId, string remark)
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                Dao.AddFormulaNutrient(id, name, dailyGain, CP, DMI, EE, CF, NFE, Ash, NDF, ADF, Starch, Ga, AllP, Arg, His, Ile, Leu, Lys, Met, Cys, Phe, Tyr, Thr, Trp, Val, P, Na, Cl, Mg, K, Fe, Cu, Mn, Zn, Se, Carotene, VE, VB1, VB2, PantothenicAcid, Niacin, Biotin, Folic, Choline, VB6, VB12, LinoleicAcid, Salt, true, principalId, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<bool> UpdateFormulaNutrient(string name, float? dailyGain, float? CP, float? DMI, float? EE, float? CF, float? NFE, float? Ash, float? NDF, float? ADF, float? Starch, float? Ga, float? AllP, float? Arg, float? His, float? Ile, float? Leu, float? Lys, float? Met, float? Cys, float? Phe, float? Tyr, float? Thr, float? Trp, float? Val, float? P, float? Na, float? Cl, float? Mg, float? K, float? Fe, float? Cu, float? Mn, float? Zn, float? Se, float? Carotene, float? VE, float? VB1, float? VB2, float? PantothenicAcid, float? Niacin, float? Biotin, float? Folic, float? Choline, float? VB6, float? VB12, float? LinoleicAcid, float? Salt, string principalId, string remark, string id)
        {
            try
            {
                if (!Dao.IsFormulaNutrientEditable(id))
                    return new ServiceResult<bool>(false, ResultStatus.ERROR, "2", "该配方不可编辑");

                Dao.UpdateFormulaNutrient(name, dailyGain, CP, DMI, EE, CF, NFE, Ash, NDF, ADF, Starch, Ga, AllP, Arg, His, Ile, Leu, Lys, Met, Cys, Phe, Tyr, Thr, Trp, Val, P, Na, Cl, Mg, K, Fe, Cu, Mn, Zn, Se, Carotene, VE, VB1, VB2, PantothenicAcid, Niacin, Biotin, Folic, Choline, VB6, VB12, LinoleicAcid, Salt, principalId, remark, id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }


        public ServiceResult<string> AddFormula(Dictionary<string, float> formulaFeed, string name, string applyTo, string sideEffect, string principalId, string operatorId, string remark)
        {
            try
            {
                if (Dao.GetFormulabyName(name).Count > 0)
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "该名称已经存在");

                string id = Guid.NewGuid().ToString();
                Dao.AddFormula(id, formulaFeed, name, applyTo, sideEffect, true, principalId, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        //public ServiceResult<bool> DeleteFormula(string id)
        //{
        //    try
        //    {
        //        Dao.UpdateFormulaStatus(true, id);
        //        return new ServiceResult<bool>(true);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
        //    }
        //}

        //public List<FeedFormulaRelevancy> GetFeedFormulaRelevancy(string formulaId)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        #region Inputs

        /// <summary>
        /// 库存是否足够出库
        /// </summary>
        /// <param name="listInOutAmount">操作日期之后的出入库记录</param>
        /// <param name="curAmount">当前库存</param>
        /// <param name="amount">需要出库的数量</param>
        /// <returns></returns>
        private bool IsEnoughToOutWarehouse(List<InOutAmount> listInOutAmount, float curAmount, float amount)
        {
            //考虑之前出入库的情况
            InOutAmount outModel = listInOutAmount.Where(t => t.Direction == InOutWarehouseDirectionEnum.Out).FirstOrDefault();
            InOutAmount inModel = listInOutAmount.Where(t => t.Direction == InOutWarehouseDirectionEnum.In).FirstOrDefault();
            //中间出库
            curAmount += outModel == null ? 0 : outModel.Amount;
            //中间入库
            curAmount -= inModel == null ? 0 : inModel.Amount;

            return curAmount < amount;
        }

        #region 饲料
        public ServiceResult<string> AddFeedInOutWarehouse(string nameId, string typeId, string areaId, float amount, DateTime operationDate, InOutWarehouseDirectionEnum direction, string principalId, string operatorId, string remark, OutWarehouseDispositonEnum? dispositon = OutWarehouseDispositonEnum.None)
        {
            try
            {
                List<Feed> list = Dao.GetFeedKindId(nameId, typeId, areaId);
                if (list.Count != 1)
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "饲料种类数量错误");//没有匹配出饲料，或者匹配出多种饲料

                //判断有没有该种饲料的库存
                object obj = Dao.GetFeedInventoryAmount(list[0].Id);
                bool hasInventory = obj != null;
                //判断库存是否足够出库
                //if (direction == InOutWarehouseDirectionEnum.Out && (!hasInventory || IsEnoughToOutWarehouse(Dao.GetFeedInOutAmount(list[0].Id, operationDate), float.Parse(obj.ToString()), amount)))
                if (direction == InOutWarehouseDirectionEnum.Out && (!hasInventory || float.Parse(obj.ToString()) < amount))
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "饲料库存不足");

                string id = Guid.NewGuid().ToString();
                Dao.AddFeedInOutWarehouse(id, list[0].Id, amount, operationDate, direction, (OutWarehouseDispositonEnum)dispositon, principalId, operatorId, DateTime.Now, remark, hasInventory);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<int> AddFeedBatchOutWarehouse(Dictionary<string, float> dictFeedAmount, List<string> shepfolds, DateTime operationDate, string principalId, string operatorId, string remark)
        {
            try
            {
                foreach (var kv in dictFeedAmount)
                {
                    //判断有没有该种饲料的库存
                    object obj = Dao.GetFeedInventoryAmount(kv.Key);
                    //判断库存是否足够出库
                    //if (obj == null || IsEnoughToOutWarehouse(Dao.GetFeedInOutAmount(kv.Key, operationDate), float.Parse(obj.ToString()), kv.Value))
                    if (obj == null || float.Parse(obj.ToString()) < kv.Value)//不考虑之前出库的状况了
                        return new ServiceResult<int>(0, ResultStatus.WANING, "2", "饲料库存不足");
                }

                //查询圈舍的所有羊只（Id,生理阶段）
                List<Sheep4FeedOutWarehouse> listSheeps = Dao.GetSheep4FeedOutWarehouse(shepfolds);

                //查询饲料的价格
                List<FeedPrice> listPrices = Dao.GetCurrentFeedPrices(dictFeedAmount.Select(t => t.Key));

                //新增出库记录
                //新增羊只采食记录
                Dao.AddFeedBatchOutWarehouse(listSheeps, listPrices, dictFeedAmount, operationDate, InOutWarehouseDirectionEnum.Out, OutWarehouseDispositonEnum.SelfUse, principalId, operatorId, DateTime.Now, remark);

                return new ServiceResult<int>(dictFeedAmount.Count);
            }
            catch (Exception ex)
            {
                return new ServiceResult<int>(0, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<string> AddAreaName(string name, string operatorId, string remark)
        {
            try
            {
                if (Dao.GetAreaNameCount(name) > 0)
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "存在相同的地区名称");

                string id = Guid.NewGuid().ToString();
                Dao.AddAreaName(id, name, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<string> AddFeedName(string name, string operatorId, string remark)
        {
            try
            {
                if (Dao.GetInputNameCount(name, feedNameCategory) > 0)
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "存在相同的饲料名称");

                string id = Guid.NewGuid().ToString();
                Dao.AddInputName(id, name, feedNameCategory, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<string> AddFeed(string feedNameId, string typeNameId, string areaId, string description, string operatorId, float? CP, float? DMI, float? EE, float? CF, float? NFE, float? Ash, float? NDF, float? ADF, float? Starch, float? Ga, float? Arg, float? His, float? Ile, float? Leu, float? Lys, float? Met, float? Cys, float? Phe, float? Tyr, float? Thr, float? Trp, float? Val, float? P, float? Na, float? Cl, float? Mg, float? K, float? Fe, float? Cu, float? Mn, float? Zn, float? Se, float? Carotene, float? VE, float? VB1, float? VB2, float? PantothenicAcid, float? Niacin, float? Biotin, float? Folic, float? Choline, float? VB6, float? VB12, float? LinoleicAcid, float? AllP)
        {
            try
            {
                if (Dao.GetFeedKindId(feedNameId, typeNameId, areaId).Count > 0)
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "该饲料已经存在");//没有匹配出饲料，或者匹配出多种饲料

                string id = Guid.NewGuid().ToString();
                Dao.AddFeed(id, feedNameId, typeNameId, areaId, description, CP, DMI, EE, CF, NFE, Ash, NDF, ADF, Starch, Ga, Arg, His, Ile, Leu, Lys, Met, Cys, Phe, Tyr, Thr, Trp, Val, P, Na, Cl, Mg, K, Fe, Cu, Mn, Zn, Se, Carotene, VE, VB1, VB2, PantothenicAcid, Niacin, Biotin, Folic, Choline, VB6, VB12, LinoleicAcid, AllP, operatorId, DateTime.Now);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<Feed> GetFeed(FeedFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetFeed(filter, pageIndex, pageSize, out totalCount);
        }
        public List<Feed> GetFeed(FeedFilter filter, int rowCount)
        {
            return Dao.GetFeed(filter, rowCount);
        }

        public Feed GetFeedByKindId(string kindId)
        {
            return Dao.GetFeedByKindId(kindId);
        }

        public FeedDetail GetFeedDetail(string kindId)
        {
            return Dao.GetFeedDetail(kindId).FirstOrDefault();
        }

        public List<FeedInOut> GetFeedInOut(FeedInOutFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetFeedInOut(filter, pageIndex, pageSize, out totalCount);
        }
        public List<FeedInOut> GetFeedInOut(FeedInOutFilter filter, int rowCount)
        {
            return Dao.GetFeedInOut(filter, rowCount);
        }

        public FeedInOut GetFeedInOutDetailById(string id)
        {
            return Dao.GetFeedInOutDetailById(id);
        }

        public List<FeedInventory> GetFeedInventory(FeedInventoryFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetFeedInventory(filter, pageIndex, pageSize, out totalCount);
        }
        public List<FeedInventory> GetFeedInventory(FeedInventoryFilter filter, int rowsCount)
        {
            return Dao.GetFeedInventory(filter, rowsCount);
        }
        public List<FeedInventory> GetFeedInventory()
        {
            return Dao.GetFeedInventory();
        }

        public ServiceResult<bool> UpdateFeed(string feedNameId, string typeNameId, string areaId, string description, string operatorId, float? CP, float? DMI, float? EE, float? CF, float? NFE, float? Ash, float? NDF, float? ADF, float? Starch, float? Ga, float? Arg, float? His, float? Ile, float? Leu, float? Lys, float? Met, float? Cys, float? Phe, float? Tyr, float? Thr, float? Trp, float? Val, float? P, float? Na, float? Cl, float? Mg, float? K, float? Fe, float? Cu, float? Mn, float? Zn, float? Se, float? Carotene, float? VE, float? VB1, float? VB2, float? PantothenicAcid, float? Niacin, float? Biotin, float? Folic, float? Choline, float? VB6, float? VB12, float? LinoleicAcid, float? AllP, string id)
        {
            try
            {
                if (!Dao.IsFeedEditable(id))
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "2", "该饲料不可编辑");

                if (Dao.ValidateFeed(id, feedNameId, typeNameId, areaId))
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "2", "该组合饲料被占用");

                Dao.UpdateFeed(feedNameId, typeNameId, areaId, description, CP, DMI, EE, CF, NFE, Ash, NDF, ADF, Starch, Ga, Arg, His, Ile, Leu, Lys, Met, Cys, Phe, Tyr, Thr, Trp, Val, P, Na, Cl, Mg, K, Fe, Cu, Mn, Zn, Se, Carotene, VE, VB1, VB2, PantothenicAcid, Niacin, Biotin, Folic, Choline, VB6, VB12, LinoleicAcid, AllP, id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<FeedWithAllFileds> GetFeedWithAllFileds()
        {
            return Dao.GetFeedWithAllFileds();
        }

        #endregion

        #region 药品
        public ServiceResult<string> AddMedicineInOutWarehouse(string nameId, string manufacturerId, string typeId, DateTime expirationDate, float amount, InOutWarehouseDirectionEnum direction, DateTime operationDate, string principalId, string operatorId, string remark)
        {
            try
            {
                List<Medicine> listMedicine = Dao.GetMedicineId(nameId, manufacturerId, typeId);//Medicine表
                if (listMedicine.Count != 1)
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "药品种类数量错误");//没有匹配出饲料，或者匹配出多种饲料

                List<Medicine> list = Dao.GetMedicineKindId(nameId, manufacturerId, typeId, expirationDate);//T_MedicineCrucial表
                string id = Guid.NewGuid().ToString();

                if (list.Count == 1)//已经存在药品
                {
                    //当前药品不存在库存的情况
                    object obj = Dao.ValidateMedicineCount(list[0].Id);
                    bool hasInventory = obj != null;

                    //判断库存是否足够出库
                    //if (direction == InOutWarehouseDirectionEnum.Out && (!hasInventory || IsEnoughToOutWarehouse(Dao.GetMedicineInOutAmount(list[0].Id, operationDate), float.Parse(obj.ToString()), amount)))
                    if (direction == InOutWarehouseDirectionEnum.Out && (!hasInventory || float.Parse(obj.ToString()) < amount))
                        return new ServiceResult<string>(null, ResultStatus.WANING, "2", "药品库存不足");

                    Dao.AddMedicineInOutWarehouse(id, list[0].Id, amount, operationDate, direction, OutWarehouseDispositonEnum.None, principalId, operatorId, DateTime.Now, remark, hasInventory);
                    return new ServiceResult<string>(id);
                }
                else if (list.Count <= 0)//不存在药品过期时间
                {
                    //判断库存是否足够出库
                    if (direction == InOutWarehouseDirectionEnum.Out)
                        return new ServiceResult<string>(null, ResultStatus.WANING, "2", "药品库存不足");

                    Dao.AddMedicineInOutWarehouse(id, listMedicine[0].Id, expirationDate, amount, operationDate, direction, OutWarehouseDispositonEnum.None, principalId, operatorId, DateTime.Now, remark);
                    return new ServiceResult<string>(id);
                }
                else
                {
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "没有该有效期对应的药品");//没有匹配出饲料，或者匹配出多种饲料
                }

            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<string> AddMedicineName(string name, string operatorId, string remark)
        {
            try
            {
                if (Dao.GetInputNameCount(name, medicineNameCategory) > 0)
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "存在相同的药品名称");

                string id = Guid.NewGuid().ToString();
                Dao.AddInputName(id, name, medicineNameCategory, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<string> AddManufacturer(string name, string department, string contactInfo, string operatorId, string principalId, string remark)
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                Dao.AddManufacturer(id, name, department, contactInfo, principalId, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<string> AddMedicine(string nameId, string manufacturerId, string typeId, string medicineUnit, string operatorId, string remark)
        {
            try
            {
                if (Dao.GetMedicineId(nameId, manufacturerId, typeId).Count > 0)
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "该药品已经存在");

                string id = Guid.NewGuid().ToString();
                Dao.AddMedicine(id, nameId, manufacturerId, typeId, medicineUnit, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<Medicine> GetMedicine(MedicineFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetMedicine(filter, pageIndex, pageSize, out totalCount);
        }
        public List<Medicine> GetMedicine(MedicineFilter filter, int rowsCount)
        {
            return Dao.GetMedicine(filter, rowsCount);
        }
        public Medicine GetMedicineByKindId(string kindId)
        {
            return Dao.GetMedicineByKindId(kindId);
        }

        public List<MedicineInOut> GetMenicineInOut(MedicineInOutFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetMenicineInOut(filter, pageIndex, pageSize, out totalCount);
        }
        public List<MedicineInOut> GetMenicineInOut(MedicineInOutFilter filter, int rowsCount)
        {
            return Dao.GetMenicineInOut(filter, rowsCount);
        }
        public MedicineInOut GetMenicineInOutDetailById(string id)
        {
            return Dao.GetMenicineInOutDetailById(id);
        }

        public List<MedicineInventory> GetMedicineInventory(MedicineInventoryFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetMedicineInventory(filter, pageIndex, pageSize, out totalCount);
        }
        public List<MedicineInventory> GetMedicineInventory(MedicineInventoryFilter filter, int rowsCount)
        {
            return Dao.GetMedicineInventory(filter, rowsCount);
        }
        public ServiceResult<bool> UpdateMedicine(string nameId, string manufacturerId, string typeId, string remark, string id)
        {
            try
            {
                if (Dao.ValidateMedicine(id, nameId, manufacturerId, typeId))
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "2", "该组合药品被占用");

                Dao.UpdateMedicine(nameId, manufacturerId, typeId, remark, id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }
        #endregion

        #region 其他

        public ServiceResult<string> AddOtherInOutWarehouse(string nameId, float amount, string unit, InOutWarehouseDirectionEnum direction, DateTime operationDate, string principalId, string operatorId, string remark, OutWarehouseDispositonEnum? dispositon = OutWarehouseDispositonEnum.None)
        {
            try
            {
                List<Other> list = Dao.GetOtherKindId(nameId);
                if (list.Count != 1)
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "物品种类数量错误");//没有匹配出饲料，或者匹配出多种饲料

                //当前其他不存在库存的情况
                object obj = Dao.ValidateOtherCount(list[0].Id);
                bool hasInventory = obj != null;

                //判断库存是否足够出库
                //if (direction == InOutWarehouseDirectionEnum.Out && (!hasInventory || IsEnoughToOutWarehouse(Dao.GetOtherInOutAmount(list[0].Id, operationDate), float.Parse(obj.ToString()), amount)))
                if (direction == InOutWarehouseDirectionEnum.Out && (!hasInventory || float.Parse(obj.ToString()) < amount))
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "该物品库存不足");

                string id = Guid.NewGuid().ToString();
                Dao.AddOtherInOutWarehouse(id, list[0].Id, amount, operationDate, direction, (OutWarehouseDispositonEnum)dispositon, principalId, operatorId, DateTime.Now, remark, hasInventory);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<string> AddOther(string name, string unit, string operatorId, string remark)
        {
            try
            {
                if (Dao.GetOtherKindId(name).Count > 0)
                    return new ServiceResult<string>(null, ResultStatus.WANING, "2", "该物品已经存在");

                string id = Guid.NewGuid().ToString();
                Dao.AddOther(id, name, unit, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<Other> GetOther(OtherFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetOther(filter, pageIndex, pageSize, out totalCount);
        }
        public List<Other> GetOther(OtherFilter filter, int rowCount)
        {
            return Dao.GetOther(filter, rowCount);
        }

        public Other GetOtherByKindId(string kindId)
        {
            return Dao.GetOtherByKindId(kindId);
        }

        public List<OtherInOut> GetOtherInOut(OtherInOutFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetOtherInOut(filter, pageIndex, pageSize, out totalCount);
        }
        public List<OtherInOut> GetOtherInOut(OtherInOutFilter filter, int rowCount)
        {
            return Dao.GetOtherInOut(filter, rowCount);
        }
        public OtherInOut GetOtherInOutDetailById(string id)
        {
            return Dao.GetOtherInOutDetailById(id);
        }

        public List<OtherInventory> GetOtherInventory(OtherInventoryFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetOtherInventory(filter, pageIndex, pageSize, out totalCount);
        }
        public List<OtherInventory> GetOtherInventory(OtherInventoryFilter filter, int rowCount)
        {
            return Dao.GetOtherInventory(filter, rowCount);
        }

        public ServiceResult<bool> UpdateOther(string name, string unit, string remark, string id)
        {
            try
            {
                if (Dao.ValidateOther(id, name))
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "2", "该组合物品被占用");

                Dao.UpdateOther(name, unit, remark, id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        #endregion

        #endregion

        #region Finance

        #region 卖
        public ServiceResult<int> AddSellSheep(List<AddSellSheep> list, decimal totalPrice, float totalWeight, string purchaserId, DateTime operationDate, string principalId, string operatorId, string remark)
        {
            try
            {
                Dao.GetSellSheepIds(list.Select(t => t.SheepId).ToList()).ForEach(s =>
                {
                    list.Remove(list.Where(t => t.SheepId.ToUpper().Equals(s.SheepId.ToUpper())).FirstOrDefault());
                });
                string sysSheepfoldId = Dao.GetSysSheepfoldId(principalId, operatorId);

                Dao.AddSellSheep(list, totalPrice, totalWeight, purchaserId, operationDate, principalId, operatorId, DateTime.Now, remark, sysSheepfoldId);
                return new ServiceResult<int>(list.Count);
            }
            catch (Exception ex)
            {
                return new ServiceResult<int>(-1, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<SellSheepBatch> GetSellSheepBath(SellSheepBatchFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetSellSheepBath(filter, pageIndex, pageSize, out totalCount);
        }
        public List<SellSheepBatch> GetSellSheepBath(SellSheepBatchFilter filter, int rowsCount)
        {
            return Dao.GetSellSheepBath(filter, rowsCount);
        }

        public List<SellSheep> GetSellSheep(SellSheepFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetSellSheep(filter, pageIndex, pageSize, out totalCount);
        }
        public List<SellSheep> GetSellSheep(SellSheepFilter filter, int rowsCount)
        {
            return Dao.GetSellSheep(filter, rowsCount);
        }

        public List<SellSheep> GetSellSheep(string batchId, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetSellSheep(batchId, pageIndex, pageSize, out totalCount);
        }

        public ServiceResult<string> AddSellManure(decimal price, string purchaserId, DateTime operationDate, string principalId, string operatorId, string remark)
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                Dao.AddSellManure(id, price, purchaserId, operationDate, principalId, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<SellManure> GetSellManure(SellManureFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetSellManure(filter, pageIndex, pageSize, out totalCount);
        }
        public List<SellManure> GetSellManure(SellManureFilter filter, int rowsCount)
        {
            return Dao.GetSellManure(filter, rowsCount);
        }

        public ServiceResult<string> AddSellWool(float amount, decimal price, string purchaserId, DateTime operationDate, string principalId, string operatorId, string remark)
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                Dao.AddSellWool(id, amount, price, purchaserId, operationDate, principalId, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<SellWool> GetSellWool(SellWoolFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetSellWool(filter, pageIndex, pageSize, out totalCount);
        }
        public List<SellWool> GetSellWool(SellWoolFilter filter, int rowsCount)
        {
            return Dao.GetSellWool(filter, rowsCount);
        }

        public ServiceResult<int> AddSellFeed(List<AddSellInput> list, string operatorId, string remark)
        {
            try
            {
                Dao.GetSellFeedIds(list.Select(t => t.LinkId).ToList()).ForEach(f =>
                {
                    list.Remove(list.Where(t => t.LinkId.ToUpper().Equals(t.LinkId.ToUpper())).FirstOrDefault());
                });

                Dao.AddSellFeed(list, operatorId, DateTime.Now, remark);
                return new ServiceResult<int>(list.Count);
            }
            catch (Exception ex)
            {
                return new ServiceResult<int>(-1, ResultStatus.ERROR, "1", ex.Message);
            }
        }
        public List<SellFeed> GetSellFeed(SellFeedFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetSellFeed(filter, pageIndex, pageSize, out totalCount);
        }
        public List<SellFeed> GetSellFeed(SellFeedFilter filter, int rowsCount)
        {
            return Dao.GetSellFeed(filter, rowsCount);
        }

        public ServiceResult<int> AddSellOther(List<AddSellInput> list, string operatorId, string remark)
        {
            try
            {
                Dao.GetSellOtherIds(list.Select(t => t.LinkId).ToList()).ForEach(f =>
                {
                    list.Remove(list.Where(t => t.LinkId.ToUpper().Equals(t.LinkId)).FirstOrDefault());
                });

                Dao.AddSellOther(list, operatorId, DateTime.Now, remark);
                return new ServiceResult<int>(list.Count);
            }
            catch (Exception ex)
            {
                return new ServiceResult<int>(-1, ResultStatus.ERROR, "1", ex.Message);
            }
        }
        public List<SellOther> GetSellOther(SellOtherFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetSellOther(filter, pageIndex, pageSize, out totalCount);
        }
        public List<SellOther> GetSellOther(SellOtherFilter filter, int rowsCount)
        {
            return Dao.GetSellOther(filter, rowsCount);
        }

        #endregion

        #region 买

        public ServiceResult<string> AddElectricCharge(float amount, decimal money, DateTime operationDate, string principalId, string operatorId, string remark)
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                Dao.AddElectricCharge(id, amount, money, operationDate, principalId, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<ElectricCharge> GetElectricCharge(ElectricChargeFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetElectricCharge(filter, pageIndex, pageSize, out totalCount);
        }
        public List<ElectricCharge> GetElectricCharge(ElectricChargeFilter filter, int rowsCount)
        {
            return Dao.GetElectricCharge(filter, rowsCount);
        }

        public ServiceResult<string> AddWaterRate(float amount, decimal money, DateTime operationDate, string principalId, string operatorId, string remark)
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                Dao.AddWaterRate(id, amount, money, operationDate, principalId, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<WaterRate> GetWaterRate(WaterRateFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetWaterRate(filter, pageIndex, pageSize, out totalCount);
        }
        public List<WaterRate> GetWaterRate(WaterRateFilter filter, int rowsCount)
        {
            return Dao.GetWaterRate(filter, rowsCount);
        }

        public ServiceResult<string> AddPayoff(string employeeId, decimal money, DateTime operationDate, string principalId, string operatorId, string remark)
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                Dao.AddPayoff(id, employeeId, money, operationDate, principalId, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<Payoff> GetPayoff(PayoffFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetPayoff(filter, pageIndex, pageSize, out totalCount);
        }
        public List<Payoff> GetPayoff(PayoffFilter filter, int rowsCount)
        {
            return Dao.GetPayoff(filter, rowsCount);
        }

        public ServiceResult<string> AddIncidentals(decimal money, DateTime operationDate, string principalId, string operatorId, string remark)
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                Dao.AddIncidentals(id, money, operationDate, principalId, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<Incidentals> GetIncidentals(IncidentalsFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetIncidentals(filter, pageIndex, pageSize, out totalCount);
        }
        public List<Incidentals> GetIncidentals(IncidentalsFilter filter, int rowsCount)
        {
            return Dao.GetIncidentals(filter, rowsCount);
        }

        public ServiceResult<string> AddBuySheep(string sheepId, string source, decimal money, DateTime operationDate, string principalId, string operatorId, string remark, float? weight)
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                Dao.AddBuySheep(id, sheepId, source, money, operationDate, principalId, operatorId, DateTime.Now, remark, weight);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<BuySheep> GetBuySheep(BuySheepFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetBuySheep(filter, pageIndex, pageSize, out totalCount);
        }
        public List<BuySheep> GetBuySheep(BuySheepFilter filter, int rowsCount)
        {
            return Dao.GetBuySheep(filter, rowsCount);
        }


        public ServiceResult<int> AddBuyFeed(Dictionary<string, decimal> inputExpenditure, DateTime operationDate, string principalId, string operatorId, string remark)
        {
            try
            {
                Dao.GetBuyFeedIds(inputExpenditure.Keys.ToList()).ForEach(k => { inputExpenditure.Remove(k.LinkId); });

                Dao.AddBuyFeed(inputExpenditure, operationDate, principalId, operatorId, DateTime.Now, remark);
                return new ServiceResult<int>(inputExpenditure.Count);
            }
            catch (Exception ex)
            {
                return new ServiceResult<int>(-1, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<BuyFeed> GetBuyFeed(BuyFeedFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetBuyFeed(filter, pageIndex, pageSize, out totalCount);
        }
        public List<BuyFeed> GetBuyFeed(BuyFeedFilter filter, int rowsCount)
        {
            return Dao.GetBuyFeed(filter, rowsCount);
        }

        public ServiceResult<int> AddBuyMedicine(Dictionary<string, decimal> inputExpenditure, DateTime operationDate, string principalId, string operatorId, string remark)
        {
            try
            {
                Dao.GetBuyMedicineIds(inputExpenditure.Keys.ToList()).ForEach(k => { inputExpenditure.Remove(k.LinkId); });

                Dao.AddBuyMedicine(inputExpenditure, operationDate, principalId, operatorId, DateTime.Now, remark);
                return new ServiceResult<int>(inputExpenditure.Count);
            }
            catch (Exception ex)
            {
                return new ServiceResult<int>(-1, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<BuyMedicine> GetBuyMedicine(BuyMedicineFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetBuyMedicine(filter, pageIndex, pageSize, out totalCount);
        }
        public List<BuyMedicine> GetBuyMedicine(BuyMedicineFilter filter, int rowsCount)
        {
            return Dao.GetBuyMedicine(filter, rowsCount);
        }

        public ServiceResult<int> AddBuyOther(Dictionary<string, decimal> inputExpenditure, DateTime operationDate, string principalId, string operatorId, string remark)
        {
            try
            {
                Dao.GetBuyOtherIds(inputExpenditure.Keys.ToList()).ForEach(k => { inputExpenditure.Remove(k.LinkId); });

                Dao.AddBuyOther(inputExpenditure, operationDate, principalId, operatorId, DateTime.Now, remark);
                return new ServiceResult<int>(inputExpenditure.Count);
            }
            catch (Exception ex)
            {
                return new ServiceResult<int>(-1, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<BuyOther> GetBuyOther(BuyOtherFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetBuyOther(filter, pageIndex, pageSize, out totalCount);
        }
        public List<BuyOther> GetBuyOther(BuyOtherFilter filter, int rowsCount)
        {
            return Dao.GetBuyOther(filter, rowsCount);
        }

        #endregion

        #endregion

        #region DecisionAids

        public List<FamilyTree> GetFamilyTree(string sheepId, int? depth = 5)
        {
            return Dao.GetFamilyTree(sheepId, (int)depth);
        }

        public List<AssistMating> GetAssistMating(string sheepId, int depth)
        {
            return Dao.GetAssistMating(sheepId, depth);
        }

        public ServiceResult<bool> VarifyTwoSheepsCanMating(string maleId, string femaleId)
        {
            try
            {
                if (maleId.ToUpper().Equals(femaleId.ToUpper()))
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "2", "相同羊只编号无法进行配种");

                List<Sheep> list = Dao.GetTwoSheeps(maleId, femaleId);

                if (list.Where(t => t.Id.ToUpper().Equals(maleId.ToUpper())).Count() <= 0)
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "2", "公羊编号不存在");

                if (list.Where(t => t.Id.ToUpper().Equals(femaleId.ToUpper())).Count() <= 0)
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "2", "母羊编号不存在");

                if (list.Select(s => s.Gender).Distinct().Count() <= 1)
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "2", "要配种的羊只性别不能相同");

                List<FamilyTree> femaleList = Dao.GetFamilyTree(femaleId, 5);//母的祖上
                List<FamilyTree> maleList = Dao.GetFamilyTree(maleId, 5);//公的祖上

                int g = 10;
                if (femaleList.Select(t => t.Id).Contains(maleId))//公的是母的祖上
                    g = femaleList.Find(t => t.Id.ToUpper().Equals(maleId.ToUpper())).Generation;
                else if (maleList.Select(t => t.Id).Contains(femaleId))//母的是公的祖上
                    g = maleList.Find(t => t.Id.ToUpper().Equals(femaleId.ToUpper())).Generation;
                else
                    femaleList.ForEach(f =>
                    {
                        if (maleList.Select(t => t.Id).Contains(f.Id) && f.Generation < g)
                            g = f.Generation;
                    });
                if (g == 10)
                    return new ServiceResult<bool>(true, ResultStatus.OK, "0");
                else
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "1", g.ToString());
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }

        }

        #endregion

        #region DiseaseControl

        public ServiceResult<string> AddAntiepidemic(string name, string vaccine, DateTime executeDate, string effect, string sheepFlock, string principalId, string operatorId, string remark)
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                Dao.AddAntiepidemic(id, name, vaccine, executeDate, effect, sheepFlock, principalId, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<Antiepidemic> GetAntiepidemic(AntiepidemicFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetAntiepidemic(filter, pageIndex, pageSize, out totalCount);
        }

        public List<Antiepidemic> GetAntiepidemic(AntiepidemicFilter filter, int rowsCount)
        {
            return Dao.GetAntiepidemic(filter, rowsCount);
        }

        public Antiepidemic GetAntiepidemicById(string id)
        {
            return Dao.GetAntiepidemicById(id);
        }

        public ServiceResult<bool> UpdateAntiepidemic(string name, string vaccine, DateTime executeDate, string effect, string sheepFlock, string principalId, string remark, string id)
        {
            try
            {
                Dao.UpdateAntiepidemic(name, vaccine, executeDate, effect, sheepFlock, principalId, remark, id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<string> AddAntiepidemicPlan(string name, string vaccine, DateTime planExecuteDate, string sheepFlock, string principalId, string operatorId, string remark)
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                Dao.AddAntiepidemicPlan(id, name, vaccine, planExecuteDate, sheepFlock, principalId, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<AntiepidemicPlan> GetAntiepidemicPlan(AntiepidemicPlanFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetAntiepidemicPlan(filter, pageIndex, pageSize, out totalCount);
        }

        public List<AntiepidemicPlan> GetAntiepidemicPlan(AntiepidemicPlanFilter filter, int rowsCount)
        {
            return Dao.GetAntiepidemicPlan(filter, rowsCount);
        }

        public AntiepidemicPlan GetAntiepidemicPlanById(string id)
        {
            return Dao.GetAntiepidemicPlanById(id);
        }

        public ServiceResult<bool> UpdateAntiepidemicPlan(string name, string vaccine, DateTime planExecuteDate, string sheepFlock, string principalId, string remark, string id)
        {
            try
            {
                Dao.UpdateAntiepidemicPlan(name, vaccine, planExecuteDate, sheepFlock, principalId, remark, id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<bool> ExecuteAntiepidemicPlan(string planId, DateTime executeDate, string effect, string principalId, string operatorId, string remark)
        {
            try
            {
                AntiepidemicPlan model = Dao.GetAntiepidemicPlanById(planId);
                if (model == null)
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "2", "防疫计划不存在");
                if (model.IsExcuted)
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "2", "防疫计划已经执行");
                if (model.CreateTime > executeDate)
                    return new ServiceResult<bool>(false, ResultStatus.WANING, "2", "计划执行时间早于计划创建时间");

                string id = Guid.NewGuid().ToString();
                Dao.ExecuteAntiepidemicPlan(id, planId, model.Name, model.Vaccine, executeDate, effect, model.SheepFlock, principalId, operatorId, DateTime.Now, remark);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public ServiceResult<string> AddTreatment(string sheepId, string symptom, DateTime startDate, string disease, string treatmentPlan, int treatmentDays, string effect, string principalId, string operatorId, string remark)
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                Dao.AddTreatment(id, sheepId, symptom, startDate, disease, treatmentPlan, treatmentDays, effect, principalId, operatorId, DateTime.Now, remark);
                return new ServiceResult<string>(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(null, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        public List<Treatment> GetTreatment(TreatmentFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            return Dao.GetTreatment(filter, pageIndex, pageSize, out totalCount);
        }

        public List<Treatment> GetTreatment(TreatmentFilter filter, int rowsCount)
        {
            return Dao.GetTreatment(filter, rowsCount);
        }

        public Treatment GetTreatmentById(string id)
        {
            return Dao.GetTreatmentById(id);
        }

        public ServiceResult<bool> UpdateTreatment(string symptom, DateTime startDate, string disease, string treatmentPlan, int treatmentDays, string effect, string principalId, string remark, string id)
        {
            try
            {
                Dao.UpdateTreatment(symptom, startDate, disease, treatmentPlan, treatmentDays, effect, principalId, remark, id);
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }

        #endregion

        #region Common
        public ServiceResult<bool> SendEmail(string subject, string body)
        {
            try
            {
                string email = ConfigurationManager.AppSettings["supportEmail"];
                string pwd = ConfigurationManager.AppSettings["supportEmailCredential"];
                string host = ConfigurationManager.AppSettings["smtpHost"];
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"]);

                using (SmtpClient smtp = new SmtpClient(host, port) { Credentials = new NetworkCredential(email, pwd) })
                {
                    using (MailMessage mail = new MailMessage(email, email, subject, body))
                    {
                        smtp.Send(mail);
                        return new ServiceResult<bool>(true);
                    }
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ResultStatus.ERROR, "1", ex.Message);
            }
        }
        #endregion

        //TODO:不确定要不要
        public ServiceResult<string> AddSheepFieldInformation(string name, string operatingRange, string qualifications, string remark)
        {
            throw new NotImplementedException();
        }

        #region 报表
        public List<MultiplyReport> GetMultiplyReport(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null)
                startDate = DateTime.Parse(DateTime.Now.Year + "-1-1");
            if (endDate == null)
                endDate = DateTime.Parse(DateTime.Now.Year + "-12-31");

            List<MultiplyReport> list = Dao.GetMultiplyReport((DateTime)startDate, (DateTime)endDate);

            MultiplyReport model = new MultiplyReport() { Month = "合计" };
            model.Abortion = list.Sum(t => t.Abortion);
            model.Delivery = list.Sum(t => t.Delivery);
            model.TotalCount = list.Sum(t => t.TotalCount);
            model.LiveMaleCount = list.Sum(t => t.LiveMaleCount);
            model.LiveFemaleCount = list.Sum(t => t.LiveFemaleCount);
            model.NormalWayCount = list.Sum(t => t.NormalWayCount);

            list.Add(model);

            list.ForEach(t =>
            {
                // --分娩率、流产率
                if (t.Delivery + t.Abortion == 0)
                {
                    t.DeliveryRate = "0";
                    t.AborationRate = "0";
                }
                else
                {
                    t.DeliveryRate = (t.Delivery * 100.0 / (t.Delivery + t.Abortion)).ToString("0.00") + "%";
                    t.AborationRate = (t.Abortion * 100.0 / (t.Delivery + t.Abortion)).ToString("0.00") + "%";
                }
                //顺产率
                t.SelfDeliveryRate = t.Delivery == 0 ? "0" : ((t.NormalWayCount * 100.0 / t.Delivery).ToString("0.00") + "%");
                //--产活率
                t.LiveRate = t.TotalCount == 0 ? "0" : (((t.LiveFemaleCount + t.LiveMaleCount) * 100.00 / t.TotalCount).ToString("0.00") + "%");

                //--公羔率、母羔率
                if (t.LiveFemaleCount + t.LiveMaleCount == 0)
                {
                    t.MaleRate = "0";
                    t.FemaleRate = "0";
                }
                else
                {
                    t.MaleRate = (t.LiveMaleCount * 100.00 / (t.LiveMaleCount + t.LiveFemaleCount)).ToString("0.00") + "%";
                    t.FemaleRate = (t.LiveFemaleCount * 100.00 / (t.LiveMaleCount + t.LiveFemaleCount)).ToString("0.00") + "%";
                }
            });
            return list;
        }


        public List<SellReport> GetSellReport(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null)
                startDate = DateTime.Parse(DateTime.Now.Year + "-1-1");
            if (endDate == null)
                endDate = DateTime.Parse(DateTime.Now.Year + "-12-31");

            List<SellReport> list = Dao.GetSellReport((DateTime)startDate, (DateTime)endDate);

            SellReport model = new SellReport() { Month = "合计" };
            model.SellFeed = list.Sum(t => t.SellFeed);
            model.SellManure = list.Sum(t => t.SellManure);
            model.SellOther = list.Sum(t => t.SellOther);
            model.SellSheep = list.Sum(t => t.SellSheep);
            model.SellWool = list.Sum(t => t.SellWool);
            model.Total = model.SellFeed + model.SellManure + model.SellOther + model.SellSheep + model.SellWool;
            list.Add(model);

            return list;
        }

        public List<BuyReport> GetBuyReport(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null)
                startDate = DateTime.Parse(DateTime.Now.Year + "-1-1");
            if (endDate == null)
                endDate = DateTime.Parse(DateTime.Now.Year + "-12-31");

            List<BuyReport> list = Dao.GetBuyReport((DateTime)startDate, (DateTime)endDate);

            BuyReport model = new BuyReport() { Month = "合计" };
            model.BuyFeed = list.Sum(t => t.BuyFeed);
            model.BuyMedicine = list.Sum(t => t.BuyMedicine);
            model.BuyOther = list.Sum(t => t.BuyOther);
            model.BuySheep = list.Sum(t => t.BuySheep);
            model.ElectricCharge = list.Sum(t => t.ElectricCharge);
            model.Payoff = list.Sum(t => t.Payoff);
            model.WaterRate = list.Sum(t => t.WaterRate);
            model.Total = model.BuyFeed + model.BuyMedicine + model.BuyOther + model.BuySheep + model.ElectricCharge + model.Payoff + model.WaterRate;
            list.Add(model);

            return list;
        }

        public List<FinanceReport> GetFinanceReport(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null)
                startDate = DateTime.Parse(DateTime.Now.Year + "-1-1");
            if (endDate == null)
                endDate = DateTime.Parse(DateTime.Now.Year + "-12-31");

            List<FinanceReport> list = Dao.GetFinanceReport((DateTime)startDate, (DateTime)endDate);
            FinanceReport model = new FinanceReport() { Month = "合计" };
            model.SellFeed = list.Sum(t => t.SellFeed);
            model.SellManure = list.Sum(t => t.SellManure);
            model.SellOther = list.Sum(t => t.SellOther);
            model.SellSheep = list.Sum(t => t.SellSheep);
            model.SellWool = list.Sum(t => t.SellWool);

            model.BuyFeed = list.Sum(t => t.BuyFeed);
            model.BuyMedicine = list.Sum(t => t.BuyMedicine);
            model.BuyOther = list.Sum(t => t.BuyOther);
            model.BuySheep = list.Sum(t => t.BuySheep);
            model.ElectricCharge = list.Sum(t => t.ElectricCharge);
            model.Payoff = list.Sum(t => t.Payoff);
            model.WaterRate = list.Sum(t => t.WaterRate);
            model.Total = (model.SellFeed + model.SellManure + model.SellOther + model.SellSheep + model.SellWool) - (model.BuyFeed + model.BuyMedicine + model.BuyOther + model.BuySheep + model.ElectricCharge + model.Payoff + model.WaterRate);

            list.Add(model);
            return list;
        }


        public List<FeedReport> GetFeedInventoryReport(DateTime? startDate, DateTime? endDate)
        {
            DateTime dtStart = startDate == null ? DateTime.Parse(DateTime.Today.Year + "-1-1") : (DateTime)startDate;
            DateTime dtEnd = endDate == null ? DateTime.Today : ((DateTime)endDate).AddMonths(1).AddDays(-1);

            List<FeedReport> tmp = Dao.GetFeedInventoryReport(dtStart, dtEnd);

            //添加没有使用量的月份
            DateTime dtTmp = dtStart;
            List<string> list = new List<string>();
            //do
            //{
            //    list.Add(dtTmp.ToString("yyyy-MM"));
            //    dtTmp = dtTmp.AddMonths(1);

            //} while (!dtTmp.ToString("yyyy-MM").Equals(dtEnd.ToString("yyyy-MM")) || dtTmp.Month < dtEnd.Month);

            while (!dtTmp.ToString("yyyy-MM").Equals(dtEnd.ToString("yyyy-MM")) || dtTmp.Month < dtEnd.Month)
            {
                list.Add(dtTmp.ToString("yyyy-MM"));
                dtTmp = dtTmp.AddMonths(1);

            }

            list.Add(dtEnd.ToString("yyyy-MM"));

            List<FeedReport> result = new List<FeedReport>();
            var names = tmp.Select(t => t.FullName).Distinct();
            foreach (var name in names)
            {
                var oneType = tmp.Where(t => t.FullName == name);
                var months = oneType.Select(t => t.Month);
                var model = oneType.FirstOrDefault();

                list.ForEach(t =>
                {
                    FeedReport newModel = new FeedReport() { Area = model.Area, Name = model.Name, Type = model.Type, Month = t };
                    if (!months.Contains(t))
                    {
                        newModel.Used = 0;
                        newModel.Storage = 0;
                    }
                    else
                    {
                        //result.Add(tmp.Find(f => f.Month == t && f.FullName == name));
                        FeedReport fri = tmp.Find(f => f.Month == t && f.FullName == name && f.Direction == InOutWarehouseDirectionEnum.In);
                        FeedReport fro = tmp.Find(f => f.Month == t && f.FullName == name && f.Direction == InOutWarehouseDirectionEnum.Out);

                        newModel.Storage = fri == null ? 0 : fri.Amount;
                        newModel.Used = fro == null ? 0 : fro.Amount;

                    }
                    result.Add(newModel);

                });
            }

            return result;
        }

        public List<FeedSheepReport> GetFeedSheepReport(string sheepId)
        {
            var result = Dao.GetFeedSheepReport(sheepId);

            if (result.Count > 0)
                result.Add(new FeedSheepReport() { Name = "合计", Area = "--", Type = "--", TotalAmount = result.Sum(t => t.TotalAmount), TotalPrice = result.Sum(t => t.TotalPrice) });

            return result;
        }

        #endregion
    }
}

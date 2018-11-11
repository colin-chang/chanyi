using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data;

using Spring.Data.Common;
using Spring.Data.Generic;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.BindingFilter;
using Chanyi.Shepherd.QueryModel;
using Chanyi.Utility.Common;

namespace Chanyi.Shepherd.Dao
{
    public partial class Dal
    {
        #region Bind

        string defaultUser = ConfigurationManager.AppSettings["defaultUser"];
        string defaultEmployee = ConfigurationManager.AppSettings["defaultEmployee"];

        public List<SheepBind> GetSheepBind(SheepBindFilter filter)
        {
            string sql = "select s.\"Id\",s.\"SerialNumber\",s.\"Gender\",s.\"BreedId\",s.\"GrowthStage\" from \"T_Sheep\" s";

            var result = GetData<SheepBind, SheepBindFilter>(sql, filter);
            return filter != null ? result : result.OrderBy(s => s.SerialNumber).ToList();
        }

        public List<SheepBind> GetSheepParentBind(string childId)
        {
            string sql = "select \"Id\",\"SerialNumber\",\"Gender\" from \"T_Sheep\" where \"Id\"<>:id and(\"GrowthStage\"=:gs1 or \"GrowthStage\"=:gs2) union select \"Id\",\"SerialNumber\",\"Gender\" from \"T_Sheep\" sm WHERE sm.\"Id\"=(select \"FatherId\" from \"T_Sheep\" where \"Id\"=:id1) union select \"Id\",\"SerialNumber\",\"Gender\" from \"T_Sheep\" sf WHERE sf.\"Id\"=(select \"MotherId\" from \"T_Sheep\" where \"Id\"=:id2) ";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", childId);
            pms.AddWithValue("gs1", (int)GrowthStageEnum.StudSheep);
            pms.AddWithValue("gs2", (int)GrowthStageEnum.TemporaryStudSheep);
            pms.AddWithValue("id1", childId);
            pms.AddWithValue("id2", childId);

            return GetData<SheepBind>(sql, pms);
        }

        public List<SheepBind> GetSheepParentBind()
        {
            string sql = "select \"Id\",\"SerialNumber\",\"Gender\" from \"T_Sheep\" where \"GrowthStage\"=:gs1 or \"GrowthStage\"=:gs2 union select \"Id\",\"SerialNumber\",\"Gender\" from \"T_Sheep\" sm WHERE sm.\"Id\" in(select \"FatherId\" from \"T_Sheep\") union select \"Id\",\"SerialNumber\",\"Gender\" from \"T_Sheep\" sf WHERE sf.\"Id\" in(select \"MotherId\" from \"T_Sheep\") ";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("gs1", (int)GrowthStageEnum.StudSheep);
            pms.AddWithValue("gs2", (int)GrowthStageEnum.TemporaryStudSheep);

            return GetData<SheepBind>(sql, pms);
        }

        public List<SheepBind> GetAbortionSheepBind(int value, int range)
        {
            string sql = "SELECT s.\"Id\", s.\"SerialNumber\" FROM \"T_Sheep\" s JOIN \"T_Mating\" M ON M .\"FemaleId\" = s.\"Id\" AND M .\"MatingDate\" + INTERVAL '" + (value + range) + " d' > now() and m.\"IsDel\"=false WHERE s.\"Status\" = :status AND s.\"Gender\" =:gender AND s.\"GrowthStage\" =:growthStage AND s.\"Id\" NOT IN ( SELECT d.\"SheepId\" FROM \"T_Delivery\" d  JOIN \"T_Mating\" ma on ma.\"FemaleId\"=d.\"SheepId\" WHERE d.\"DeliveryDate\" + INTERVAL '" + (value - range) + " d' > now() and ma.\"IsDel\"=false and d.\"IsDel\"=false group by d.\"SheepId\" HAVING \"max\" (d.\"DeliveryDate\") > \"max\" (ma.\"MatingDate\") UNION SELECT A .\"SheepId\" FROM \"T_Abortion\" A JOIN \"T_Mating\" ma ON ma.\"FemaleId\" = A .\"SheepId\" WHERE A .\"AbortionDate\" + INTERVAL '" + (-value - range) + " d' > now()  and a.\"IsDel\"=false and ma.\"IsDel\"=false GROUP BY A .\"SheepId\" HAVING \"max\" (A .\"AbortionDate\") > \"max\" (ma.\"MatingDate\")) GROUP BY s.\"Id\", s.\"SerialNumber\" ORDER BY s.\"SerialNumber\"";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("status", (int)SheepStatusEnum.Nomal);
            pms.AddWithValue("gender", (int)GenderEnum.Female);
            pms.AddWithValue("growthStage", (int)GrowthStageEnum.StudSheep);

            return GetData<SheepBind>(sql, pms);
        }

        public List<SheepBind> GetDeliverySheepBind(int value, int range)
        {
            string sql = "SELECT s.\"Id\", s.\"SerialNumber\" FROM \"T_Sheep\" s JOIN \"T_Mating\" M ON M .\"FemaleId\" = s.\"Id\" AND M .\"MatingDate\" + INTERVAL '" + (value + range) + " d' > now() and M .\"MatingDate\" + INTERVAL '" + (value - range) + " d' < now() and m.\"IsDel\"=false WHERE s.\"Status\" = :status AND s.\"Gender\" =:gender AND s.\"GrowthStage\" =:growthStage AND s.\"Id\" NOT IN ( SELECT d.\"SheepId\" FROM \"T_Delivery\" d  JOIN \"T_Mating\" ma on ma.\"FemaleId\"=d.\"SheepId\" WHERE d.\"DeliveryDate\" + INTERVAL '" + (value - range) + " d' > now() and ma.\"IsDel\"=false and d.\"IsDel\"=false group by d.\"SheepId\" HAVING \"max\" (d.\"DeliveryDate\") > \"max\" (ma.\"MatingDate\") UNION SELECT A .\"SheepId\" FROM \"T_Abortion\" A JOIN \"T_Mating\" ma ON ma.\"FemaleId\" = A .\"SheepId\" WHERE A .\"AbortionDate\" + INTERVAL '" + (value - range) + " d' > now()  and a.\"IsDel\"=false and ma.\"IsDel\"=false GROUP BY A .\"SheepId\" HAVING \"max\" (A .\"AbortionDate\") > \"max\" (ma.\"MatingDate\")) GROUP BY s.\"Id\", s.\"SerialNumber\" ORDER BY s.\"SerialNumber\"";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("status", (int)SheepStatusEnum.Nomal);
            pms.AddWithValue("gender", (int)GenderEnum.Female);
            pms.AddWithValue("growthStage", (int)GrowthStageEnum.StudSheep);

            return GetData<SheepBind>(sql, pms);
        }
        public List<SheepBind> GetMatingSheepSelectBind()
        {
            string sql = "select DISTINCT(s.\"Id\"),s.\"SerialNumber\",s.\"Gender\" from \"T_Mating\" m join \"T_Sheep\" s on m.\"FemaleId\"=s.\"Id\"  where m.\"IsDel\"=FALSE union SELECT DISTINCT(s.\"Id\"),s.\"SerialNumber\",s.\"Gender\" from \"T_Mating\" m join \"T_Sheep\" s on m.\"MaleId\"=s.\"Id\"  where m.\"IsDel\"=FALSE";

            return GetData<SheepBind>(sql, null);
        }
        public List<SheepBind> GetMatingSheepCountBind()
        {
            string sql = "select s.\"Id\",s.\"SerialNumber\" from \"T_Sheep\" s where s.\"Gender\"=:gender and (s.\"GrowthStage\"=:g1 or s.\"GrowthStage\"=:g2) union  SELECT s.\"Id\",s.\"SerialNumber\" from \"T_Sheep\" s join \"T_ExceptAssess\" e on s.\"Id\"=e.\"SheepId\" where e.\"IsDel\"=false  and s.\"Gender\"=:genderUnion and (e.\"SourceGrowthStage\"=:g3 or e.\"SourceGrowthStage\"=:g4)";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("gender", (int)GenderEnum.Female);
            pms.AddWithValue("g1", (int)GrowthStageEnum.StudSheep);
            pms.AddWithValue("g2", (int)GrowthStageEnum.TemporaryStudSheep);

            pms.AddWithValue("genderUnion", (int)GenderEnum.Female);
            pms.AddWithValue("g3", (int)GrowthStageEnum.StudSheep);
            pms.AddWithValue("g4", (int)GrowthStageEnum.TemporaryStudSheep);

            return GetData<SheepBind>(sql, pms);
        }
        public List<SheepBind> GetStudSheepBind()
        {
            string sql = "SELECT \"Id\",\"SerialNumber\",\"Gender\" FROM \"T_Sheep\" where \"Status\"=:status and \"GrowthStage\" in ('" + (int)GrowthStageEnum.StudSheep + "','" + (int)GrowthStageEnum.TemporaryStudSheep + "') order by \"SerialNumber\"";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("status", (int)SheepStatusEnum.Nomal);

            return GetData<SheepBind>(sql, pms);
        }

        public List<SheepBind> GetStudSheepBindWithOuter()
        {
            string sql = "SELECT \"Id\",\"SerialNumber\",\"Gender\" FROM \"T_Sheep\" where (\"Status\"="+(int)SheepStatusEnum.Nomal+" or \"Status\"="+(int)SheepStatusEnum.Outer+") and \"GrowthStage\" in ('" + (int)GrowthStageEnum.StudSheep + "','" + (int)GrowthStageEnum.TemporaryStudSheep + "') order by \"SerialNumber\"";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("status", (int)SheepStatusEnum.Nomal);

            return GetData<SheepBind>(sql, pms);
        }

        public List<SheepBind> GetAbortionSheepSelectBind()
        {
            string sql = "select  DISTINCT(s.\"Id\"),s.\"SerialNumber\" from \"T_Abortion\" a join \"T_Sheep\" s on a.\"SheepId\"=s.\"Id\" where a.\"IsDel\"=FALSE";

            return GetData<SheepBind>(sql, null);
        }

        public List<SheepBind> GetDeliverySheepSelectBind()
        {
            string sql = "select DISTINCT(s.\"Id\"),s.\"SerialNumber\" from \"T_Delivery\" d join \"T_Sheep\" s on d.\"SheepId\"=s.\"Id\"  where d.\"IsDel\"=FALSE";

            return GetData<SheepBind>(sql, null);
        }

        public List<SheepBind> GetStudAssessSheepSelectBind()
        {
            string sql = "select DISTINCT(s.\"Id\"),s.\"SerialNumber\" from \"T_AssessStudsheep\" ass join \"T_Assess\" a on a.\"Id\"=ass.\"AssessId\" join \"T_Sheep\" s on s.\"Id\"=a.\"SheepId\"";

            return GetData<SheepBind>(sql, null);
        }

        public List<SheepBind> GetFirstAssessSheepSelectBind()
        {
            string sql = "select DISTINCT(s.\"Id\"),s.\"SerialNumber\" from \"T_FirstAssess\" ass join \"T_Assess\" a on a.\"Id\"=ass.\"AssessId\" join \"T_Sheep\" s on s.\"Id\"=a.\"SheepId\"";

            return GetData<SheepBind>(sql, null);
        }

        public List<SheepBind> GetDeathSheepSelectBind()
        {
            string sql = "select DISTINCT(s.\"Id\"),s.\"SerialNumber\" from \"T_DeathManage\" d join \"T_Sheep\" s on d.\"SheepId\"=s.\"Id\"  where d.\"IsDel\"=FALSE";

            return GetData<SheepBind>(sql, null);
        }

        public List<SheepBind> GetSecondAssessSheepAddBind()
        {
            string sql = "SELECT s.\"Id\", s.\"SerialNumber\" FROM \"T_Sheep\" s WHERE s.\"Status\" =:status AND S.\"GrowthStage\"=:growthStage AND s.\"Id\" NOT IN (SELECT A .\"SheepId\" FROM \"T_SecondAssess\" sa JOIN \"T_Assess\" A ON A .\"Id\" = sa.\"AssessId\") ORDER BY s.\"SerialNumber\"";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("status", (int)SheepStatusEnum.Nomal);
            pms.AddWithValue("growthStage", (int)GrowthStageEnum.LambHog);

            return GetData<SheepBind>(sql, pms);
        }

        public List<SheepBind> GetSecondAssessSheepSelectBind()
        {
            string sql = "select s.\"Id\",s.\"SerialNumber\" from \"T_SecondAssess\" sa join \"T_Assess\" a on a.\"Id\"=sa.\"AssessId\" join \"T_Sheep\" s on s.\"Id\"=a.\"SheepId\" ORDER BY a.\"AssessDate\" DESC";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("status", (int)SheepStatusEnum.Nomal);

            return GetData<SheepBind>(sql, pms);
        }

        public List<SheepBind> GetThirdAssessSheepAddBind()
        {
            string sql = "SELECT s.\"Id\", s.\"SerialNumber\" FROM \"T_Sheep\" s WHERE s.\"Status\" =:status AND S.\"GrowthStage\"=:growthStage AND s.\"Id\" NOT IN (SELECT A .\"SheepId\" FROM \"T_ThirdAssess\" sa JOIN \"T_Assess\" A ON A.\"Id\" = sa.\"AssessId\") ORDER BY s.\"SerialNumber\"";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("status", (int)SheepStatusEnum.Nomal);
            pms.AddWithValue("growthStage", (int)GrowthStageEnum.TemporaryStudSheep);

            return GetData<SheepBind>(sql, pms);
        }

        public List<SheepBind> GetThirdAssessSheepSelectBind()
        {
            string sql = "select s.\"Id\",s.\"SerialNumber\" from \"T_ThirdAssess\" ta join \"T_Assess\" a on a.\"Id\"=ta.\"AssessId\" join \"T_Sheep\" s on s.\"Id\"=a.\"SheepId\" ORDER BY a.\"AssessDate\" DESC";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("status", (int)SheepStatusEnum.Nomal);

            return GetData<SheepBind>(sql, pms);
        }

        public List<SheepBind> GetExceptAssessSheepAddBind()
        {
            string sql = "SELECT \"Id\",\"SerialNumber\" from \"T_Sheep\" where \"GrowthStage\"<>:growthStage and \"Status\"=:status";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("growthStage", (int)GrowthStageEnum.FattingSheep);
            pms.AddWithValue("status", (int)SheepStatusEnum.Nomal);

            return GetData<SheepBind>(sql, pms);
        }
        public List<SheepBind> GetExceptAssessSheepSelectBind()
        {
            string sql = "SELECT s.\"Id\",s.\"SerialNumber\" from \"T_ExceptAssess\" e join \"T_Sheep\" s on s.\"Id\"=e.\"SheepId\"";
            return GetData<SheepBind>(sql, null);
        }

        public List<SheepBind> GetTreatmentSheepBind()
        {
            string sql = "select s.\"Id\",s.\"SerialNumber\" from \"T_Treatment\" t join \"T_Sheep\" s on t.\"SheepId\"=s.\"Id\"";

            return GetData<SheepBind>(sql, null);
        }

        public List<BreedBind> GetBreedBind()
        {
            string sql = "select b.\"Id\",b.\"Name\" from \"T_Breed\" b Order by b.\"Name\"";
            return GetData<BreedBind>(sql, null);
        }

        public List<SheepfoldBind> GetSheepfoldBind()
        {
            string sql = "select sf.\"Id\",sf.\"Name\" from \"T_Sheepfold\" sf where sf.\"SysFlag\"=FALSE  ORDER BY sf.\"Name\"";
            return GetData<SheepfoldBind>(sql, null);
        }

        public List<SheepfoldSheepCountBind> GetSheepfoldSheepCountBind()
        {
            string sql = "SELECT DISTINCT sf.\"Id\", sf.\"Name\", COUNT (s.\"Id\") OVER (PARTITION BY sf.\"Id\") AS \"Count\" FROM \"T_Sheepfold\" AS sf JOIN \"T_Sheep\" s ON sf.\"Id\" = s.\"SheepfoldId\" WHERE sf.\"SysFlag\" = FALSE";
            return GetData<SheepfoldSheepCountBind>(sql, null);
        }

        /// <summary>
        /// 在职员工绑定
        /// </summary>
        /// <returns></returns>
        public List<EmployeeBind> GetEmployeeBind()
        {
            //string sql = "select e.\"Id\",e.\"Name\",e.\"SerialNum\" from \"T_Employee\" e where e.\"Status\"=:status and \"Name\"<>'" + defaultEmployee + "' Order by e.\"Name\"";
            string sql = "select e.\"Id\",e.\"Name\",e.\"SerialNum\" from \"T_Employee\" e where e.\"Status\"=:status  Order by e.\"Name\"";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("status", (int)EmployeeStatusEnum.OnJob);
            return GetData<EmployeeBind>(sql, pms);
        }

        public List<EmployeeBind> GetAllEmployeeBind()
        {
            string sql = "select e.\"Id\",e.\"Name\",e.\"SerialNum\" from \"T_Employee\" e  where \"Name\"<>'" + defaultEmployee + "' Order by e.\"Name\"";
            return GetData<EmployeeBind>(sql, null);
        }

        public List<UserBind> GetUserBind()
        {
            string sql = "select u.\"Id\",u.\"UserName\" from \"T_User\" u where u.\"IsEnabled\"=true and \"UserName\"<>'" + defaultUser + "' Order by u.\"UserName\"";
            return GetData<UserBind>(sql, null);
        }

        public List<DutyBind> GetDutyBind()
        {
            string sql = "select d.\"Id\",d.\"Name\" from \"T_Duty\" d";
            IDbParameters pms = AdoTemplate.CreateDbParameters();

            return GetData<DutyBind>(sql, pms);
        }

        public Dictionary<SheepfoldBind, List<SheepBind>> GetMoveSheepfoldBind()
        {
            string sql = "select s.\"Id\",s.\"SerialNumber\",sf.\"Id\" as \"SheepfoldId\",sf.\"Name\" as \"SheepfoldName\" from \"T_Sheepfold\" sf left join \"T_Sheep\" s on s.\"SheepfoldId\"=sf.\"Id\" where s.\"Status\"=:status and  sf.\"SysFlag\"=FALSE order by sf.\"Name\"";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("status", (int)SheepStatusEnum.Nomal);

            List<SheepBind> list = GetData<SheepBind>(sql, pms);
            Dictionary<SheepfoldBind, List<SheepBind>> dict = new Dictionary<SheepfoldBind, List<SheepBind>>();

            var l = list.GroupBy(s => s.SheepfoldId).ToList();
            l.ForEach(s =>
            {
                List<SheepBind> temp = s.ToList();
                dict.Add(new SheepfoldBind { Id = temp[0].SheepfoldId, Name = temp[0].SheepfoldName }, string.IsNullOrEmpty(temp[0].Id) ? new List<SheepBind>() : temp.Select(t => new SheepBind { Id = t.Id, SerialNumber = t.SerialNumber }).ToList());
            });

            return dict;
        }

        public Dictionary<DiseaseTypeBind, List<DiseaseBind>> GetDiseaseTypeBind()
        {
            string sql = "select dt.\"TypeId\",t.\"Name\" as \"TypeName\",d.\"SicknessId\" as \"Id\",s.\"Name\" from \"T_DiseaseType\" dt join \"T_Type\" t on t.\"Id\"=dt.\"TypeId\" join \"T_Sickness\" s on s.\"TypeId\"=dt.\"TypeId\" join \"T_Disease\" d on d.\"SicknessId\"=s.\"Id\" ORDER BY t.\"Name\"";

            List<DiseaseBind> list = GetData<DiseaseBind>(sql, null);
            Dictionary<DiseaseTypeBind, List<DiseaseBind>> dict = new Dictionary<DiseaseTypeBind, List<DiseaseBind>>();

            list.GroupBy(d => d.TypeName).ToList().ForEach(d =>
            {
                List<DiseaseBind> temp = d.ToList();
                dict.Add(new DiseaseTypeBind { Id = temp[0].TypeId, Name = temp[0].TypeName }, string.IsNullOrEmpty(temp[0].Id) ? new List<DiseaseBind>() : temp.Select(t => new DiseaseBind { Id = t.Id, Name = t.Name }).ToList());
            });

            return dict;
        }

        public List<DiseaseBind> GetDiseaseBindBySymptomIds(string[] symptomIds)
        {
            string sql = "SELECT sk.\"Id\", sk.\"Name\", \"count\" (sm.\"SicknessId\") AS \"Relevancy\" FROM \"T_Disease\" d JOIN \"T_Sickness\" sk ON sk.\"Id\" = d.\"SicknessId\" JOIN \"T_DiseaseSymptom\" ds ON ds.\"DiseaseId\" = sk.\"Id\" JOIN \"T_Symptom\" sm ON sm.\"SicknessId\" = ds.\"SymptomId\" JOIN \"T_Sickness\" ss ON ss.\"Id\" = sm.\"SicknessId\" WHERE sm.\"SicknessId\" IN ('" + string.Join("','", symptomIds) + "') GROUP BY sk.\"Id\", sk.\"Name\" HAVING \"count\" (sm.\"SicknessId\") > 0 ORDER BY \"count\" (sm.\"SicknessId\") desc";

            return GetData<DiseaseBind>(sql, null);
        }

        public List<DiseaseBind> GetDiseaseBindByName(string diseaseName)
        {
            string sql = "select s.\"Id\",s.\"Name\" from \"T_Disease\" d join \"T_Sickness\" s on s.\"Id\"=d.\"SicknessId\" WHERE s.\"Name\" LIKE :diseaseName ";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("diseaseName", diseaseName.Wrap("%"));

            return GetData<DiseaseBind>(sql, pms);
        }

        public Dictionary<SymptomTypeBind, List<SymptomBind>> GetSymptomTypeBind()
        {
            string sql = "select st.\"TypeId\",t.\"Name\" as \"TypeName\",s.\"SicknessId\" as \"Id\",sk.\"Name\" from \"T_SymptomType\"  st join \"T_Type\" t on t.\"Id\"=st.\"TypeId\" join \"T_Sickness\" sk on sk.\"TypeId\"=st.\"TypeId\" join \"T_Symptom\" s on s.\"SicknessId\"=sk.\"Id\" ORDER BY t.\"Name\"";

            List<SymptomBind> list = GetData<SymptomBind>(sql, null);
            Dictionary<SymptomTypeBind, List<SymptomBind>> dict = new Dictionary<SymptomTypeBind, List<SymptomBind>>();

            list.GroupBy(d => d.TypeName).ToList().ForEach(d =>
            {
                List<SymptomBind> temp = d.ToList();
                dict.Add(new SymptomTypeBind { Id = temp[0].TypeId, Name = temp[0].TypeName }, string.IsNullOrEmpty(temp[0].Id) ? new List<SymptomBind>() : temp.Select(t => new SymptomBind { Id = t.Id, Name = t.Name }).ToList());
            });

            return dict;
        }

        public List<SymptomBind> GetSymptomBind(SymptomBindFilter filter)
        {
            string sql = "select s.\"Id\",s.\"Name\" from \"T_Symptom\" sm join \"T_Sickness\" s on s.\"Id\"=sm.\"SicknessId\"";

            return GetData<SymptomBind, SymptomBindFilter>(sql, filter);
        }

        public List<PurchaserBind> GetPurchaserBind()
        {
            string sql = "select c.\"Id\",c.\"Name\",c.\"Department\" from \"T_Purchaser\" p join \"T_Cooperater\" c on c.\"Id\"=p.\"CooperaterId\"";

            return GetData<PurchaserBind>(sql, null);
        }

        public List<FeedNameBind> GetFeedNameBind()
        {
            string sql = "select \"Id\",\"Name\" from \"T_InputName\" where \"Category\"=:category";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("category", feedNameCategory);

            return GetData<FeedNameBind>(sql, pms);
        }
        public List<FeedTypeBind> GetFeedTypeBind()
        {
            string sql = "select \"Id\",\"Value\" as \"Name\" from \"T_InventoryDict\" where \"Category\"=:category ORDER BY \"Id\"";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("category", feedTypeCategory);

            return GetData<FeedTypeBind>(sql, pms);
        }
        public List<FeedTypeBind> GetFeedTypeBind(string feedNameId)
        {
            string sql = "select \"Id\",\"Value\" as \"Name\" from \"T_InventoryDict\" where \"Category\"=:category and \"Id\" in (select \"TypeId\" from \"T_Feed\"  WHERE \"NameId\"=:feedNameId GROUP BY \"TypeId\") ORDER BY \"Id\"";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("category", feedTypeCategory);
            pms.AddWithValue("feedNameId", feedNameId);

            return GetData<FeedTypeBind>(sql, pms);
        }
        public List<AreaBind> GetAreaBind()
        {
            string sql = "select \"Id\",\"Name\" from \"T_Area\"";

            return GetData<AreaBind>(sql, null);
        }
        public List<AreaBind> GetAreaBind(string feedNameId, string typeId)
        {
            string sql = "select \"Id\",\"Name\" from \"T_Area\" where \"Id\" in (select \"AreaId\" from \"T_Feed\"  WHERE \"NameId\"=:feedNameId and \"TypeId\"=:typeId GROUP BY \"AreaId\")";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("feedNameId", feedNameId);
            pms.AddWithValue("typeId", typeId);

            return GetData<AreaBind>(sql, pms);
        }

        public List<MedicineBind> GetMedicineNameBind()
        {
            string sql = "select \"Id\",\"Name\" from \"T_InputName\" where \"Category\"=:category";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("category", medicineNameCategory);

            return GetData<MedicineBind>(sql, pms);
        }
        public List<ManufactureBind> GetManufactureBind()
        {
            string sql = "select c.\"Id\",c.\"Department\" as \"Name\" from \"T_Manufacture\" m join \"T_Cooperater\" c on m.\"CooperaterId\"=c.\"Id\"";

            return GetData<ManufactureBind>(sql, null);
        }
        public List<ManufactureBind> GetManufactureBind(string medicineNameId)
        {
            string sql = "select c.\"Id\",c.\"Name\" from \"T_Manufacture\" m join \"T_Cooperater\" c on c.\"Id\"=m.\"CooperaterId\" where m.\"CooperaterId\" in (select \"ManufacturerId\" from \"T_Medicine\" where \"NameId\"=:medicineNameId)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("medicineNameId", medicineNameId);

            return GetData<ManufactureBind>(sql, pms);
        }
        public List<DepartmentBind> GetDepartmentBind()
        {
            string sql = "select c.\"Id\",c.\"Department\" as \"Name\" from \"T_Manufacture\" m join \"T_Cooperater\" c on m.\"CooperaterId\"=c.\"Id\"";

            return GetData<DepartmentBind>(sql, null);
        }
        public List<DepartmentBind> GetDepartmentBind(string medicineNameId)
        {
            string sql = "select c.\"Id\",c.\"Department\" as \"Name\" from \"T_Manufacture\" m join \"T_Cooperater\" c on c.\"Id\"=m.\"CooperaterId\" where m.\"CooperaterId\" in (select \"ManufacturerId\" from \"T_Medicine\" where \"NameId\"=:medicineNameId)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("medicineNameId", medicineNameId);

            return GetData<DepartmentBind>(sql, pms);
        }
        public List<MedicineTypeBind> GetMedicineTypeBind()
        {
            string sql = "select \"Id\",\"Value\" as \"Name\" from \"T_InventoryDict\" where \"Category\"=:category ORDER BY \"Id\"";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("category", medicineTypeCategory);

            return GetData<MedicineTypeBind>(sql, pms);
        }


        public List<MedicineTypeBind> GetMedicineTypeBind(string MedicineNameId)
        {
            string sql = "select d.\"Id\",\"Value\" as \"Name\" from \"T_Medicine\" m join \"T_InventoryDict\" d on m.\"TypeId\"=d.\"Id\" where m.\"NameId\"=:nameId and d.\"Category\"=:category order by d.\"Id\"";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("nameId", MedicineNameId);
            pms.AddWithValue("category", medicineTypeCategory);

            return GetData<MedicineTypeBind>(sql, pms);
        }

        public List<CooperaterBind> GetManufacturerBind(string MedicineNameId, string typeId)
        {
            string sql = "select c.\"Id\",\"Department\" as \"Name\" from \"T_Medicine\" m join \"T_Manufacture\" mf  on m.\"ManufacturerId\"=mf.\"CooperaterId\" join \"T_Cooperater\" c on c.\"Id\"=mf.\"CooperaterId\" where m.\"NameId\"=:nameId and m.\"TypeId\"=:typeId order by c.\"Id\"";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("nameId", MedicineNameId);
            pms.AddWithValue("category", medicineTypeCategory);
            pms.AddWithValue("typeId", typeId);

            return GetData<CooperaterBind>(sql, pms);
        }

        public List<OtherBind> GetOtherBind()
        {
            string sql = "select o.\"Id\",o.\"Name\",od.\"Unit\" from \"T_Other\" o join \"T_OtherDetail\" od on od.\"KindId\"=o.\"Id\"";

            return GetData<OtherBind>(sql, null);
        }

        public List<SheepBind> GetBuySheepBind4Add()
        {
            string sql = "select s.\"Id\",s.\"SerialNumber\" from \"T_Sheep\" s where s.\"Origin\"=1 and s.\"Id\" not in (select \"SheepId\" from \"T_BuySheep\") order by s.\"CreateTime\" DESC";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("origin", (int)OriginEnum.Purchase);

            return GetData<SheepBind>(sql, pms);
        }

        public List<SheepBind> GetBuySheepBind4Select()
        {
            string sql = "select s.\"Id\",s.\"SerialNumber\" from \"T_Sheep\" s join \"T_BuySheep\" bs on bs.\"SheepId\"=s.\"Id\" where s.\"Origin\"=:origin order by s.\"CreateTime\" DESC";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("origin", (int)OriginEnum.Purchase);

            return GetData<SheepBind>(sql, pms);
        }

        public List<BuyFeedBind> GetBuyFeedBind4Add()
        {
            string sql = "select w.\"Id\",n.\"Name\",a.\"Name\" as \"Area\",d.\"Value\" as \"Type\",w.\"Amount\",w.\"OperationDate\" from  \"T_InOutWarehouse\" w join \"T_Feed\" f on f.\"Id\"=w.\"KindId\" join \"T_InputName\" n on n.\"Id\"=f.\"NameId\" left join \"T_Area\" a on a.\"Id\"=f.\"AreaId\" join \"T_InventoryDict\" d on d.\"Id\"=f.\"TypeId\" where w.\"Id\" not in (select \"LinkId\" from \"T_BuyFeed\") and w.\"Direction\"=:direction and d.\"Category\"=:category";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("direction", (int)InOutWarehouseDirectionEnum.In);
            pms.AddWithValue("category", feedTypeCategory);
            return GetData<BuyFeedBind>(sql, pms);
        }
        public List<BuyMedicineBind> GetBuyMedicineBind4Add()
        {
            string sql = "select w.\"Id\",n.\"Name\",c.\"Name\" as \"Manufacturer\",w.\"Amount\",w.\"OperationDate\" from \"T_InOutWarehouse\" w join \"T_MedicineCrucial\" mc on mc.\"Id\"=w.\"KindId\" join \"T_Medicine\" m on m.\"Id\"=mc.\"KindId\" join \"T_InputName\" n on n.\"Id\"=m.\"NameId\" join \"T_Cooperater\" c on c.\"Id\"=m.\"ManufacturerId\" where w.\"Id\" not in (SELECT \"LinkId\" from \"T_BuyMedicine\") and w.\"Direction\"=:direction";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("direction", (int)InOutWarehouseDirectionEnum.In);

            return GetData<BuyMedicineBind>(sql, pms);
        }
        public List<BuyOtherBind> GetBuyOtherBind4Add()
        {
            string sql = "select w.\"Id\",o.\"Name\",w.\"Amount\",w.\"OperationDate\" from \"T_InOutWarehouse\" w join \"T_Other\" o on o.\"Id\"=w.\"KindId\" where w.\"Id\" not in (select \"LinkId\" from \"T_BuyOther\") and w.\"Direction\"=:direction";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("direction", (int)InOutWarehouseDirectionEnum.In);

            return GetData<BuyOtherBind>(sql, pms);
        }

        public List<SellFeedBind> GetSellFeedBind4Add()
        {
            string sql = "select w.\"Id\",n.\"Name\",a.\"Name\" as \"Area\",d.\"Value\" as \"Type\",w.\"Amount\",w.\"OperationDate\" from  \"T_InOutWarehouse\" w join \"T_Feed\" f on f.\"Id\"=w.\"KindId\" join \"T_InputName\" n on n.\"Id\"=f.\"NameId\" left join \"T_Area\" a on a.\"Id\"=f.\"AreaId\" join \"T_InventoryDict\" d on d.\"Id\"=f.\"TypeId\" where w.\"Id\" not in (select \"LinkId\" from \"T_SellFeed\") and w.\"Direction\"=:direction and d.\"Category\"=:category and w.\"Dispositon\"=:dispositon";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("direction", (int)InOutWarehouseDirectionEnum.Out);
            pms.AddWithValue("category", feedTypeCategory);
            pms.AddWithValue("dispositon", (int)OutWarehouseDispositonEnum.Sell);
            return GetData<SellFeedBind>(sql, pms);
        }
        public List<SellOtherBind> GetSellOtherBind4Add()
        {
            string sql = "select w.\"Id\",o.\"Name\",w.\"Amount\",w.\"OperationDate\" from \"T_InOutWarehouse\" w join \"T_Other\" o on o.\"Id\"=w.\"KindId\" where w.\"Id\" not in (select \"LinkId\" from \"T_SellOther\") and w.\"Direction\"=:direction and w.\"Dispositon\"=:dispositon";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("direction", (int)InOutWarehouseDirectionEnum.Out);
            pms.AddWithValue("dispositon", (int)OutWarehouseDispositonEnum.Sell);

            return GetData<SellOtherBind>(sql, pms);
        }

        public List<FormulaBind> GetFormulaBind(bool isEnable)
        {
            string sql = "select f.\"Id\",f.\"Name\",f.\"ApplyTo\",f.\"SideEffect\" from \"T_Formula\" f where f.\"IsEnable\"=:isEnable";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("isEnable", isEnable);

            return GetData<FormulaBind>(sql, pms);
        }

        #endregion
    }
}

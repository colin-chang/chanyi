using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.Dao.Dals
{
    public partial class Dal
    {
        #region Assist

        //public List<DiseaseType> GetDiseaseType(string pid)
        //{
        //    string sql = "select t.\"Id\",t.\"Name\" from \"T_DiseaseType\" dt left join \"T_Type\" t on t.\"Id\"=dt.\"TypeId\" WHERE dt.\"Pid\"=:pid;";
        //    IDbParameters pms = AdoTemplate.CreateDbParameters();
        //    pms.AddWithValue("pid", pid);
        //    return GetData<DiseaseType>(sql, pms);
        //}

        //public List<Disease> GetDiseaseByType(string typeId)
        //{
        //    string sql = "select s.\"Id\",s.\"Name\" from \"T_Disease\" d JOIN \"T_Sickness\" s on s.\"Id\"=d.\"SicknessId\" where s.\"TypeId\"=:typeId";
        //    IDbParameters pms = AdoTemplate.CreateDbParameters();
        //    pms.AddWithValue("typeId", typeId);
        //    return GetData<Disease>(sql, pms);
        //}

        //public List<Disease> GetDiseaseBySymptomName(string symptomName)
        //{
        //    string sql = "SELECT DISTINCT(ds.\"Id\"), ds.\"Name\" FROM \"T_Disease\" d JOIN \"T_Sickness\" ds ON ds.\"Id\" = d.\"SicknessId\" JOIN \"T_DiseaseSymptom\" dsm ON dsm.\"DiseaseId\" = d.\"SicknessId\" WHERE dsm.\"SymptomId\" IN ( SELECT ss.\"Id\" FROM \"T_Symptom\" s JOIN \"T_Sickness\" ss ON ss.\"Id\" = s.\"SicknessId\" WHERE ss.\"Name\" LIKE :symptomName )";

        //    IDbParameters pms = AdoTemplate.CreateDbParameters();
        //    pms.AddWithValue("symptomName", symptomName.Wrap("%"));

        //    return GetData<Disease>(sql, pms);
        //}

        //public List<Disease> GetCrossDiseaseBySymptomIds(string[] symptomIds)
        //{
        //    string sql = "select sk.\"Id\",sk.\"Name\",ss.\"Id\" as \"SymptomId\" from \"T_Disease\" d join \"T_Sickness\" sk on sk.\"Id\"=d.\"SicknessId\" join \"T_DiseaseSymptom\" ds on ds.\"DiseaseId\"=sk.\"Id\" join \"T_Symptom\" sm on sm.\"SicknessId\"=ds.\"SymptomId\" join \"T_Sickness\" ss on ss.\"Id\"=sm.\"SicknessId\"";

        //    List<Disease> result = new List<Disease>();
        //    GetData<Disease>(sql, null).GroupBy(d => d.Id).ToList().ForEach(d =>
        //    {
        //        List<string> list = d.Select(s => s.SymptomId).ToList();
        //        if (list.Except(symptomIds).Count() == list.Count - symptomIds.Count())
        //            result.Add(d.ToList()[0]);
        //    });

        //    return result;
        //}


        #endregion
    }
}

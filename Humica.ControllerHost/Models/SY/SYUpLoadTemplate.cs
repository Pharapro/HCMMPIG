using Humica.Core.DB;
using Humica.EF.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Humica.Models.SY
{
    public class SYUpLoadTemplate
    {
        IUnitOfWork unitOfWork;
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>();
        }
        public SYUpLoadTemplate() {
            OnLoad();
        }
        public List<MDUploadTemplate> GetData(string SCREEN_ID,string UserName)
        {
            List<MDUploadTemplate> templates = new List<MDUploadTemplate>();
            templates = unitOfWork.Set<MDUploadTemplate>().Where(w=> w.ScreenId == SCREEN_ID &&
            w.UploadBy == UserName).OrderByDescending(w => w.UploadDate).ToList();
            return templates;
        }
    }
}
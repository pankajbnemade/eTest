using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Master;
using ERP.Services.Master.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Master
{
    public class FormService : Repository<Form>, IForm
    {
        public FormService(ErpDbContext dbContext) : base(dbContext) { }

        public async Task<int> CreateForm(FormModel formModel)
        {
            int formId = 0;

            // assign values.
            Form form = new Form();

            form.FormName = formModel.FormName;
            form.ModuleId = formModel.ModuleId;
           
            formId = await Create(form);

            return formId; // returns.
        }

        public async Task<bool> UpdateForm(FormModel formModel)
        {
            bool isUpdated = false;

            // get record.
            Form form = await GetByIdAsync(w => w.FormId == formModel.FormId);
            if (null != form)
            {
                // assign values.
                form.FormName = formModel.FormName;
                form.ModuleId = formModel.ModuleId;

                isUpdated = await Update(form);
            }

            return isUpdated; // returns.
        }


        public async Task<bool> DeleteForm(int formId)
        {
            bool isDeleted = false;

            // get record.
            Form form = await GetByIdAsync(w => w.FormId == formId);
            if (null != form)
            {
                isDeleted = await Delete(form);
            }

            return isDeleted; // returns.
        }


        public async Task<FormModel> GetFormById(int formId)
        {
            FormModel formModel = null;

            IList<FormModel> formModelList = await GetFormList(formId);
            if (null != formModelList && formModelList.Any())
            {
                formModel = formModelList.FirstOrDefault();
            }

            return formModel; // returns.
        }

        public async Task<DataTableResultModel<FormModel>> GetFormList()
        {
            DataTableResultModel<FormModel> resultModel = new DataTableResultModel<FormModel>();

            IList<FormModel> formModelList = await GetFormList(0);
            if (null != formModelList && formModelList.Any())
            {
                resultModel = new DataTableResultModel<FormModel>();
                resultModel.ResultList = formModelList;
                resultModel.TotalResultCount = formModelList.Count();
            }

            return resultModel; // returns.
        }


        /// <summary>
        /// get all form list.
        /// </summary>
        /// <returns>
        /// return list.
        /// </returns>
        public async Task<IList<FormModel>> GetFormList(int formId)
        {
            IList<FormModel> formModelList = null;

            // get records by query.

            IQueryable<Form> query = GetQueryByCondition(w => w.FormId != 0).Include(w => w.PreparedByUser).Include(w => w.Module);

            if (0 != formId)
                query = query.Where(w => w.FormId == formId);

            IList<Form> formList = await query.ToListAsync();

            if (null != formList && formList.Count > 0)
            {
                formModelList = new List<FormModel>();
                foreach (Form form in formList)
                {
                    formModelList.Add(await AssignValueToModel(form));
                }
            }

            return formModelList; // returns.
        }

        private async Task<FormModel> AssignValueToModel(Form form)
        {
            return await Task.Run(() =>
            {
                FormModel formModel = new FormModel();

                formModel.FormId = form.FormId;
                formModel.FormName = form.FormName;
                formModel.ModuleId = form.ModuleId;

                formModel.PreparedByName = form.PreparedByUser.UserName;

                return formModel;
            });
        }

        public async Task<IList<SelectListModel>> GetFormSelectList()
        {
            IList<SelectListModel> resultModel = null;

            if (await Any(w => w.FormId != 0))
            {
                IQueryable<Form> query = GetQueryByCondition(w => w.FormId != 0);

                resultModel = await query
                                    .Select(s => new SelectListModel
                                    {
                                        DisplayText = s.FormName,
                                        Value = s.FormId.ToString()
                                    })
                                    .ToListAsync();
            }

            return resultModel; // returns.
        }


    }
}

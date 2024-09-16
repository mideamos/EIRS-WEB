using EIRS.API.Models;
using EIRS.BOL;
using EIRS.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace EIRS.API.Controllers
{
    public class BaseController : ApiController
    {
        public APIResponse PaginationList<T>(IList<T> lstData, PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            if (pobjPagingModel != null)
            {
                // Get's No of Rows Count   
                int count = lstData.Count();

                // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
                int CurrentPage = pobjPagingModel.pageNumber;

                // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
                int PageSize = pobjPagingModel.pageSize;

                // Display TotalCount to Records to User  
                int TotalCount = count;

                // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                // Returns List of Customer after applying Paging   
                var items = lstData.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

                // if CurrentPage is greater than 1 means it has previousPage  
                var previousPage = CurrentPage > 1 ? "Yes" : "No";

                // if TotalPages is greater than CurrentPage means it has nextPage  
                var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

                // Object which we are going to send in header   
                var paginationMetadata = new
                {
                    totalCount = TotalCount,
                    pageSize = PageSize,
                    currentPage = CurrentPage,
                    totalPages = TotalPages,
                    previousPage,
                    nextPage
                };

                // Setting Header  
                HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = items;
            }
            else
            {
                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstData;
            }

            return mObjAPIResponse;
        }
        public APIResponse PaginationListII<T>(IList<T> lstData, NewPagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            if (pobjPagingModel != null)
            {
                // Get's No of Rows Count   
                int count = lstData.Count();

                // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
                int CurrentPage = pobjPagingModel.pageNumber;

                // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
                int PageSize = pobjPagingModel.pageSize;

                // Display TotalCount to Records to User  
                int TotalCount = count;

                // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                var items = new List<T>();
                // Returns List of Customer after applying Paging   
                if (count > PageSize)
                    items = lstData.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
                else
                {
                    items = lstData.Take(count).ToList();
                    PageSize = count;
                    CurrentPage = 1;
                    TotalPages = 1;
                }
                // if CurrentPage is greater than 1 means it has previousPage  
                var previousPage = CurrentPage > 1 ? "Yes" : "No";

                // if TotalPages is greater than CurrentPage means it has nextPage  
                var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

                // Object which we are going to send in header   
                var paginationMetadata = new
                {
                    totalCount = TotalCount,
                    pageSize = PageSize,
                    currentPage = CurrentPage,
                    totalPages = TotalPages,
                    previousPage,
                    items,
                    nextPage
                };

                // Setting Header  
                //HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = paginationMetadata;
            }
            else
            {
                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstData;
            }

            return mObjAPIResponse;
        }
    }
}

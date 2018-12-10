﻿using Sundar.BLL;
using Sundar.BLL.Request;
using Sundar.BLL.Response;
using Sundar.Common;
using Sundar.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;

namespace Sundar.MVC.Controllers
{
    public class UserManagerController : BaseController
    {

        public UserManagerBLL App { get; set; }

        /// <summary>
        /// 用户管理主页
        /// </summary>
        /// <returns></returns>
        [Authenticate]
        public ActionResult Index()
        {
            return View();
        }

        //添加或修改组织
        [System.Web.Mvc.HttpPost]
        public string AddOrUpdate(UserView view)
        {
            try
            {
                App.AddOrUpdate(view);
            }
            catch (Exception ex)
            {
                Result.Code = 500;
                Result.Message = ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }

        /// <summary>
        /// 加载组织下面的所有用户
        /// </summary>
        public string Load([FromUri]QueryUserListReq request)
        {
            return JsonHelper.Instance.Serialize(App.Load(request));
        }

        [System.Web.Mvc.HttpPost]
        public string Delete(int[] ids)
        {
            try
            {
                App.Delete(ids);
            }
            catch (Exception e)
            {
                Result.Code = 500;
                Result.Message = e.Message;
            }

            return JsonHelper.Instance.Serialize(Result);
        }

        #region 获取权限数据

        /// <summary>
        /// 获取用户可访问的账号
        /// <para>李玉宝于2017-02-28 15:12:19</para>
        /// </summary>
        public string GetAccessedUsers()
        {
            IEnumerable<UserView> users = App.Load(new QueryUserListReq()).data;
            var result = new Dictionary<int, object>();
            foreach (var user in users)
            {
                var item = new
                {
                    Account = user.Account,
                    RealName = user.Name,
                    id = user.Id,
                    text = user.Name,
                    value = user.Account,
                    parentId = "0",
                    showcheck = true,
                    img = "fa fa-user",
                };
                result.Add(user.Id, item);
            }

            return JsonHelper.Instance.Serialize(result);
        }
        #endregion
    }
}
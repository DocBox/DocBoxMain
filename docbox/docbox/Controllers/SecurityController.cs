﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;

namespace docbox.Controllers
{
    public class SecurityController : Controller
    {
        //
        // GET: /Security/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GenerateKey()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GenerateKey(string password)
        {
            string passphrase = Request.Params.Get("passph");
            byte[] passData = new byte[passphrase.Length * sizeof(char)];
            System.Buffer.BlockCopy(passphrase.ToCharArray(), 0, passData, 0, passData.Length);
            SHA256 shaHash = new SHA256Managed();
            byte[] hashData = shaHash.ComputeHash(passData);

            Response.Clear();
            // Add a HTTP header to the output stream that specifies the default filename
            // for the browser's download dialog
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "key.sha256");
            // Add a HTTP header to the output stream that contains the 
            // content length(File Size). This lets the browser know how much data is being transfered
            Response.AddHeader("Content-Length", hashData.Length.ToString());
            // Set the HTTP MIME type of the output stream
            Response.ContentType = "application/octet-stream";

            Response.BinaryWrite(hashData);
            Response.Flush();

            return View();
        }
    }
}
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.Collections.Generic;

namespace docbox.Models
{
    public class DocumentBinder : DefaultModelBinder
    {
        //protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        //{
        //    Type type = modelType;
        //    if (modelType.IsGenericType)
        //    {
        //        Type genericTypeDefinition = modelType.GetGenericTypeDefinition();
        //        if (genericTypeDefinition == typeof(IDictionary<,>))
        //        {
        //            type = typeof(Dictionary<,>).MakeGenericType(modelType.GetGenericArguments());
        //        }
        //        else if (((genericTypeDefinition == typeof(IEnumerable<>)) || (genericTypeDefinition == typeof(ICollection<>))) || (genericTypeDefinition == typeof(IList<>)))
        //        {
        //            type = typeof(List<>).MakeGenericType(modelType.GetGenericArguments());
        //        }
        //        return Activator.CreateInstance(type);
        //    }
        //    else if (modelType.IsAbstract)
        //    {
        //        string concreteTypeName = bindingContext.ModelName + ".Type";
        //        var concreteTypeResult = bindingContext.ValueProvider.GetValue(concreteTypeName);

        //        if (concreteTypeResult == null)
        //            throw new Exception("Concrete type for abstract class not specified");

        //        type = Assembly.GetExecutingAssembly().GetTypes().SingleOrDefault(t => t.IsSubclassOf(modelType) && t.Name == concreteTypeResult.AttemptedValue);

        //        if (type == null)
        //            throw new Exception(String.Format("Concrete model type {0} not found", concreteTypeResult.AttemptedValue));

        //        var instance = Activator.CreateInstance(type);
        //        bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => instance, type);
        //        return instance;
        //    }
        //    else
        //    {
        //        return Activator.CreateInstance(modelType);
        //    }
        //}

        //public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        //{
        //    var dictionaryBindingContext = new ModelBindingContext()
        //    {
        //        ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => null, typeof(IDictionary<object,object>)),
        //        ModelName = "FileName,Description,Owner,CreationDate,checkLock", //The name(s) of the form elements you want going into the dictionary
        //        ModelState = bindingContext.ModelState,
        //        PropertyFilter = bindingContext.PropertyFilter,
        //        ValueProvider = bindingContext.ValueProvider
        //    };

        //    var model = base.BindModel(controllerContext, dictionaryBindingContext);
        //    //var model = base.BindModel(controllerContext, bindingContext);

        //    return model;
        //}

        //public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        //{

        //    var model = base.BindModel(controllerContext, bindingContext) as Files; //Bind most of the model using the built-in binder, this will bind all primitives for us   
        //    model.files.
        //    const string magicString = "files";  //TODO Use of magic strings  

        //    var files = bindingContext.ValueProvider.GetValue(magicString); //Get the posted value for files  

        //    if (files != null && !string.IsNullOrEmpty(files.AttemptedValue))
        //    { //Check we have a value for files before we proceed  

        //        bindingContext.ModelState.Remove(magicString); //Remove binding conversion errors for files, as we are going to deal with binding files ourselves  

        //        try
        //        {
        //            //Create a list of files based on the comma delimited string of posted OfficeId's  
        //            model.files = new List(
        //                                                files.AttemptedValue.Split(",".ToCharArray())
        //                                                .Select(id => new FileModel() { })
        //                                                .ToList()
        //                                            );
        //        }
        //        catch (FormatException ex)
        //        { //Catch if the posted files are not posted as a comma delimited string of Guids  
        //            bindingContext.ModelState.AddModelError(magicString, ex); //Add an error to the model state, used for ModelState.IsValid and Html error helpers  
        //        }
        //        catch (Exception ex)
        //        { //Unexpected exception  
        //            throw ex;
        //        }
        //    }

        //    return model;
        //}  
  
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor)
        {

            //if (propertyDescriptor.PropertyType == typeof(List<FileModel>))
            //{
            //    var fileList = new List<FileModel>();
            //    var form = controllerContext.HttpContext.Request.Form;
            //    var keys = form.Keys;
            //    foreach (var key in keys)
            //    {

            //    }
            //}

            //{
            //    var incomingData = bindingContext.ValueProvider.GetValue("Edit." + propertyDescriptor.Name + "[]");
            //    if (incomingData != null)
            //    {
            //        List<FileModel> fileModels = new List<FileModel>();
            //        fileModels = (List<FileModel>)incomingData.ConvertTo(typeof(List<FileModel>));
            //        var model = bindingContext.Model as Files;
            //        model.files = fileModels;
            //    }
            //}

            if (propertyDescriptor.Name == "Archive")
            {
                var list = new List<int>(5);
                var form = controllerContext.HttpContext.Request.Form;
                var ids = form.AllKeys.Where(x => x.StartsWith("id"));

                foreach (var id in ids)
                {
                    int i;
                    if (int.TryParse(form.Get(id), out i))
                    {
                        list.Add(i);
                    }
                }
                SetProperty(controllerContext, bindingContext, propertyDescriptor, list);
            }
            if(propertyDescriptor.Name == "Search")
            {
                propertyDescriptor.SetValue(bindingContext.Model, controllerContext.HttpContext.Request.Form["filename"]);
            }
            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }
    }
}
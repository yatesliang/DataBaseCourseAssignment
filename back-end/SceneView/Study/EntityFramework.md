## EntityFramework

- ### ORM 对象关系映射

- ### EDM 实体数据模型

  - 概念模型
  - 存储模型
  - 映射

- ### DBContext

  ​	实体类与数据库之间的桥梁，负责与数据进行交互

- ### CRUD操作

  ​	即增删改查

- ### ICollection<>在构造函数中会被初始化为HashSet<>

- ### 实体的状态

  > 实体在其生存期内，只有一个状态，每一个状态都有一个对应的SQL命令，以下是状态分类

  - Added => insert创建新行进行持久化

  - Deleted  => 已存在对应的行，并delete

  - Modified -- 已经被修改 => 已存在对应的行，并update

  - Unchanged -- 还未被修改 => 对数据库没有影响，表示没有修改的东西需要持久化 

  - Detached -- 不能被追踪 => 对数据库没有影响

    > ![1528736338418](http://getme.guitoubing.top/1528736338418.png)
    >
    > ![1528736384833](http://getme.guitoubing.top/1528736384833.png)

# Razor解析

- #### Action Selector

  - ActionName - 指定与方法名称不同的操作名称

  - NonAction - 用于指定某方法不是Action Method（浅意理解为无需绑定试图）

  - ActionVerbs - 默认为Get请求

    在发放前使用[ActionVerbs]属性，说明方法执行的动作。

    ActionVerbs有以下类型：

    > ![ActionVerbs](http://getme.guitoubing.top/QQ截图20180614000504.png)

- ### HtmlHelper

  > HtmlHelper class generates html elements using the model class object in razor view. It binds the model object to html elements to display value of model properties into html elements and also assigns the value of the html elements to the model properties while submitting web form. So always use HtmlHelper class in razor view instead of writing html tags manually. 

  - HtmlHelper方法及对应的Html控件

  > ![HtmlHelper方法及对应的Html控件](http://getme.guitoubing.top/QQ截图20180614001429.png)

  - TextBox

    - Html.TextBox

    > ```c#
    > @model Student
    > @Html.TextBox("StudentName", null, new { @class = "form-control" })  
    > ```
    >
    > =>
    >
    > ```html
    > <input class="form-control" 
    >         id="StudentName" 
    >         name="StudentName" 
    >         type="text" 
    >         value="" />
    > ```

    - Html.TextBoxFor

    > ```c#
    > @model Student
    > @Html.TextBoxFor(m => m.StudentName, new { @class = "form-control" })  
    > ```
    >
    > =>
    >
    > ```html
    > <input class="form-control" 
    >         id="StudentName" 
    >         name="StudentName" 
    >         type="text" 
    >         value="John" />
    > ```

  - TextArea

    - Html.TextArea

    > ```c#
    > @model Student
    > @Html.TextArea("myTextArea", "This is value", new { @class = "form-control" })  
    > ```
    >
    > =>
    >
    > ```html
    > <textarea class="form-control" 
    >             cols="20" 
    >             id="myTextArea" 
    >             name="myTextArea" 
    >             rows="2">This is value</textarea>
    > ```

    - Html.TextArea

    > ```c#
    > @model Student
    > @Html.TextAreaFor(m => m.Description, new { @class = "form-control" })  
    > ```
    >
    > =>
    >
    > ```html
    > <textarea class="form-control" 
    >             cols="20" 
    >             id="Description" 
    >             name="Description" 
    >             rows="2"></textarea>
    > ```

  - DropDownList

    - Html.DropDownList

    > ```c#
    > @using MyMVCApp.Models
    > @model Student
    > @Html.DropDownList("StudentGender", 
    >                     new SelectList(Enum.GetValues(typeof(Gender))),
    >                     "Select Gender",
    >                     new { @class = "form-control" })
    > ```
    >
    > =>
    >
    > ```html
    > <select class="form-control" id="StudentGender" name="StudentGender">
    >     <option>Select Gender</option> 
    >     <option>Male</option> 
    >     <option>Female</option> 
    > </select>
    > ```

    - Html.DropDownListFor

    > ```c#
    > @using MyMVCApp.Models
    > @model Student
    > @Html.DropDownListFor(m => m.StudentGender, 
    >                     new SelectList(Enum.GetValues(typeof(Gender))), 
    >                     "Select Gender")
    > ```
    >
    > =>
    >
    > ```html
    > <select class="form-control" id="StudentGender" name="StudentGender">
    >     <option>Select Gender</option> 
    >     <option>Male</option> 
    >     <option>Female</option> 
    > </select>
    > ```

  - Password

    > 与TextBox类似，创建的input块type为password。

  - Editor

    > Editor和EditorFor会根据参数类型创建特定的input块
    >
    > ![Editor/EditorFor的参数类型及其Html转换](http://getme.guitoubing.top/QQ截图20180614004544.png)

    - Html.Editor

    > ```
    > StudentId:      @Html.Editor("StudentId")
    > Student Name:   @Html.Editor("StudentName")
    > Age:            @Html.Editor("Age")
    > Password:       @Html.Editor("Password")
    > isNewlyEnrolled: @Html.Editor("isNewlyEnrolled")
    > Gender:         @Html.Editor("Gender")
    > DoB:            @Html.Editor("DoB")
    > ```

    - Html.EditorFor

    > ```
    > StudentId:      @Html.EditorFor(m => m.StudentId)
    > Student Name:   @Html.EditorFor(m => m.StudentName)
    > Age:            @Html.EditorFor(m => m.Age)
    > Password:       @Html.EditorFor(m => m.Password)
    > isNewlyEnrolled: @Html.EditorFor(m => m.isNewlyEnrolled)
    > Gender:         @Html.EditorFor(m => m.Gender)
    > DoB:            @Html.EditorFor(m => m.DoB)
    > ```

- ### 模型绑定

  - 传统的http请求

    > ![传统的Http请求](http://getme.guitoubing.top/request-data.png)

  - MVC使用的模型绑定（我们尽量避免使用Get方法，因此这里就只学习Post）

    - Model-class-binding

      > ![Model-class-binding](http://getme.guitoubing.top/model-class-binding.png)

    - FormCollection

      > 不使用现有模型，而采用FormCollection包装数据

  - 属性包含和排除

    > 使用属性包含和排除，可以仅仅绑定需要的属性来提高性能。 
    >
    > ```c#
    > [HttpPost]
    > public ActionResult Edit([Bind(Include = "StudentId, StudentName")] Student std)
    > {
    >     var name = std.StudentName;
    >     //write code to update student 
    >     return RedirectToAction("Index");
    > }
    > ```
    >
    > ```c#
    > [HttpPost]
    > public ActionResult Edit([Bind(Exclude = "Age")] Student std)
    > {
    >     var name = std.StudentName;
    >     //write code to update student 
    >     return RedirectToAction("Index");
    > }
    > ```

- ### 数据验证 - DataAnnotations

  - DataAnnotation属性

    > ![DataAnnotations验证属性](http://getme.guitoubing.top/QQ截图20180614011144.png)

  - 添加DataAnnotation属性 

    > DataAnnotation声明在Models/模型类中，同时在声明后可添加自定义错误信息例如：
    >
    > ```c#
    > [Required(ErrorMessage="Please enter student name.")]
    > ```
    >
    > ```c#
    > public class Student
    > {
    >     public int StudentId { get; set; }
    >      
    >     [Required]
    >     public string StudentName { get; set; }
    >        
    >     [Range(5,50)]
    >     public int Age { get; set; }
    > }
    > ```

  - 视图中的写法

    > 1. 使用**ValidationSummary**显示视图中的所有错误消息。
    > 2. 使用**ValidationMessageFor**或**ValidationMessage**帮助器方法在视图中显示字段级错误消息。
    > 3. 使用**ModelState.IsValid**在操作方法中更新之前检查模型是否有效 
    >
    > **StudentController.cs**
    >
    > ```c#
    > [HttpPost]
    > public ActionResult Edit(Student std)
    > {
    >     if (ModelState.IsValid) { 
    >         //write code to update student 
    >         return RedirectToAction("Index");
    >     }
    >     return View(std);
    > }
    > ```
    >
    > **Edit.cshtml：**
    >
    > ```HTML
    > @model MVC_BasicTutorials.Models.Student
    > 
    > @{
    >     ViewBag.Title = "Edit";
    >     Layout = "~/Views/Shared/_Layout.cshtml";
    > }
    > 
    > <h2>Edit</h2>
    > 
    > @using (Html.BeginForm())
    > {
    >     @Html.AntiForgeryToken()
    >     
    >     <div class="form-horizontal">
    >         <h4>Student</h4>
    >         <hr />
    >         @Html.ValidationSummary(true, "", new { @class = "text-danger" })
    >         @Html.HiddenFor(model => model.StudentId)
    > 
    >         <div class="form-group">
    >             @Html.LabelFor(model => model.StudentName, htmlAttributes: new { @class = "control-label col-md-2" })
    >             <div class="col-md-10">
    >                 @Html.EditorFor(model => model.StudentName, new { htmlAttributes = new { @class = "form-control" } })
    >                 @Html.ValidationMessageFor(model => model.StudentName, "", new { @class = "text-danger" })
    >             </div>
    >         </div>
    > 
    >         <div class="form-group">
    >             @Html.LabelFor(model => model.Age, htmlAttributes: new { @class = "control-label col-md-2" })
    >             <div class="col-md-10">
    >                 @Html.EditorFor(model => model.Age, new { htmlAttributes = new { @class = "form-control" } })
    >                 @Html.ValidationMessageFor(model => model.Age, "", new { @class = "text-danger" })
    >             </div>
    >         </div>
    > 
    >         <div class="form-group">
    >             <div class="col-md-offset-2 col-md-10">
    >                 <input type="submit" value="Save" class="btn btn-default" />
    >             </div>
    >         </div>
    >     </div>
    > }
    > 
    > <div>
    >     @Html.ActionLink("Back to List", "Index")
    > </div>
    > ```
    >
    > ![Validation-editview](http://getme.guitoubing.top/validation-editview.png)
    >
    > 

  - ValidationSummary

    > 如上所述，ValidationSummary用于显示所有字段的错误信息，也可以用其显示自定义错误信息，该方法会生成ModelStateDictionary对象中的验证消息的无序列表（ul元素）。 
    >
    > **示例：在ModelState中添加错误**
    >
    > ```c#
    > if (ModelState.IsValid) { 
    >               
    >     //check whether name is already exists in the database or not
    >     bool nameAlreadyExists = * check database *       
    >         
    >     if(nameAlreadyExists)
    >     {
    >         ModelState.AddModelError(string.Empty, "Student Name already exists.");
    >     
    >         return View(std);
    >     }
    > }
    > ```
    >
    > ![validationsummary-demo2](http://getme.guitoubing.top/validationsummary-demo2.png)
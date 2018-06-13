# EntityFramework

- ORM 对象关系映射

- EDM 实体数据模型

  - 概念模型
  - 存储模型
  - 映射

- DBContext

  ​	实体类与数据库之间的桥梁，负责与数据进行交互

- CRUD操作

  ​	即增删改查

- ICollection<>在构造函数中会被初始化为HashSet<>

- 实体的状态

  > 实体在其生存期内，只有一个状态，每一个状态都有一个对应的SQL命令，以下是状态分类

  - Added => insert创建新行进行持久化

  - Deleted  => 已存在对应的行，并delete

  - Modified -- 已经被修改 => 已存在对应的行，并update

  - Unchanged -- 还未被修改 => 对数据库没有影响，表示没有修改的东西需要持久化 

  - Detached -- 不能被追踪 => 对数据库没有影响

    > ![1528736338418](C:\Users\Tanrui\AppData\Local\Temp\1528736338418.png)
    >
    > ![1528736384833](C:\Users\Tanrui\AppData\Local\Temp\1528736384833.png)


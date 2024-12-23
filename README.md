# Plastic SCM to Git

> $ cm fast-export The-Adventure-of-Roo@Roo@cloud repo.fe.00
> 
> $ mv repo.fe.00 [git directory]

**[On git directory]**

> $ [do what you want] ...
>
> $ git push origin main

# add LFS and Migrates LFS

> $ git lfs migrate import --include-"[file_path]"
> 
> $ git push origin main
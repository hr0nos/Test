//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IMAGE2
{
    using System;
    using System.Collections.Generic;
    
    public partial class Filmcountry
    {
        public int id { get; set; }
        public int id_film { get; set; }
        public int id_country { get; set; }
    
        public virtual film_country film_country { get; set; }
        public virtual Proba_film Proba_film { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.IO;

namespace HotelOffice.Data
{
    /*
     * هذا الكلاس مخصص فقط لأدوات سطر الأوامر مثل Add-Migration.
     * وظيفته هي إخبار أداة EF Core بكيفية إنشاء DbContext في وقت التصميم.
     */
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // بما أننا في وقت التصميم، يمكننا استخدام أي مسار صالح لقاعدة البيانات.
            // الأداة لا تهتم *ببيانات* قاعدة البيانات، بل بـ "تصميمها" فقط.
            // سنستخدم مسارًا مؤقتًا هنا.
            optionsBuilder.UseSqlite("Data Source=temp_design_time.db");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
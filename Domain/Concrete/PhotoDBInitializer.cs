using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Domain.Entities;

namespace Domain.Concrete
{
    public class PhotoDBInitializer : DropCreateDatabaseIfModelChanges<PhotoDBContext>
    {
        protected override void Seed(PhotoDBContext context)
        {
            var categories = new List<Category>
                {
                    new Category {CategoryID = 1, CategoryName = "Fashion",ShortName = "cat1",CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 1},
                    new Category {CategoryID = 2, CategoryName = "Model Test",ShortName = "cat2", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 2},
                    new Category {CategoryID = 3, CategoryName = "Clothing Catalog",ShortName = "cat3", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 3},
                    new Category {CategoryID = 4, CategoryName = "Wedding",ShortName = "cat4", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 4},
                    new Category {CategoryID = 5, CategoryName = "Reportage Filming",ShortName = "cat5", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 5},

                };

            categories.ForEach(x => context.Categories.Add(x));
            context.SaveChanges();
            


            var products = new List<Product>
                {
                    //new Product {ProductID = 1, CategoryID = 1, ImgExt = "test1", StartDate = DateTime.Now, UpdateDate = DateTime.Now},
                    //new Product {ProductID = 2, CategoryID = 1, ImgExt = "test2", StartDate = DateTime.Now, UpdateDate = DateTime.Now},
                    //new Product {ProductID = 3, CategoryID = 1, ImgExt = "test3", StartDate = DateTime.Now, UpdateDate = DateTime.Now},
                    //new Product {ProductID = 4, CategoryID = 1, ImgExt = "test4", StartDate = DateTime.Now, UpdateDate = DateTime.Now},
                    //new Product {ProductID = 5, CategoryID = 1, ImgExt = "test5", StartDate = DateTime.Now, UpdateDate = DateTime.Now},
                    //new Product {ProductID = 6, CategoryID = 2, ImgExt = "test6", StartDate = DateTime.Now, UpdateDate = DateTime.Now},
                    //new Product {ProductID = 7, CategoryID = 2, ImgExt = "test7", StartDate = DateTime.Now, UpdateDate = DateTime.Now},
                    //new Product {ProductID = 8, CategoryID = 2, ImgExt = "test8", StartDate = DateTime.Now, UpdateDate = DateTime.Now},
                    //new Product {ProductID = 9, CategoryID = 2, ImgExt = "test9", StartDate = DateTime.Now, UpdateDate = DateTime.Now},
                    //new Product {ProductID = 10, CategoryID = 3, ImgExt = "test10", StartDate = DateTime.Now, UpdateDate = DateTime.Now},
                    //new Product {ProductID = 11, CategoryID = 3, ImgExt = "test11", StartDate = DateTime.Now, UpdateDate = DateTime.Now},
                    //new Product {ProductID = 12, CategoryID = 3, ImgExt = "test12", StartDate = DateTime.Now, UpdateDate = DateTime.Now},
                    //new Product {ProductID = 13, CategoryID = 3, ImgExt = "test13", StartDate = DateTime.Now, UpdateDate = DateTime.Now},
                    //new Product {ProductID = 14, CategoryID = 3, ImgExt = "test14", StartDate = DateTime.Now, UpdateDate = DateTime.Now},
                    //new Product {ProductID = 15, CategoryID = 3, ImgExt = "test15", StartDate = DateTime.Now, UpdateDate = DateTime.Now},
                    //new Product {ProductID = 16, CategoryID = 4, ImgExt = "test16", StartDate = DateTime.Now, UpdateDate = DateTime.Now},
                    //new Product {ProductID = 17, CategoryID = 4, ImgExt = "test17", StartDate = DateTime.Now, UpdateDate = DateTime.Now},
                    //new Product {ProductID = 18, CategoryID = 4, ImgExt = "test18", StartDate = DateTime.Now, UpdateDate = DateTime.Now},
                    //new Product {ProductID = 19, CategoryID = 4, ImgExt = "test19", StartDate = DateTime.Now, UpdateDate = DateTime.Now},
                    //new Product {ProductID = 20, CategoryID = 4, ImgExt = "test20", StartDate = DateTime.Now, UpdateDate = DateTime.Now},
                    //new Product {ProductID = 21, CategoryID = 4, ImgExt = "test21", StartDate = DateTime.Now, UpdateDate = DateTime.Now},
                    //new Product {ProductID = 22, CategoryID = 5, ImgExt = "test22", StartDate = DateTime.Now, UpdateDate = DateTime.Now},
                    //new Product {ProductID = 23, CategoryID = 5, ImgExt = "test23", StartDate = DateTime.Now, UpdateDate = DateTime.Now},
                    //new Product {ProductID = 24, CategoryID = 5, ImgExt = "test24", StartDate = DateTime.Now, UpdateDate = DateTime.Now},
                    //new Product {ProductID = 25, CategoryID = 5, ImgExt = "test25", StartDate = DateTime.Now, UpdateDate = DateTime.Now},
                    //new Product {ProductID = 26, CategoryID = 5, ImgExt = "test26", StartDate = DateTime.Now, UpdateDate = DateTime.Now},
                    //new Product {ProductID = 27, CategoryID = 5, ImgExt = "test27", StartDate = DateTime.Now, UpdateDate = DateTime.Now}
                    
                };

            products.ForEach(x => context.Products.Add(x));
            context.SaveChanges();


            var roles = new List<Role>
                {
                    new Role {RoleID = 1, RoleName = "User"},
                    new Role {RoleID = 2, RoleName = "Admin"}
                    

                };

            roles.ForEach(x => context.Roles.Add(x));
            context.SaveChanges();



            var users = new List<User>
                {
                    new User {UserID = 1, RoleID = 2, Created = DateTime.Now, IsActivated = true, Email = "graf43479@ya.ru",Login = "graf43479", Password = "FE608D7BDE2CB2B40D924C2DEA75541248BE79DA", PasswordSalt = "xdW7DAsL5fgit0ZbFPXDubAuVCJm+IAk0SEDrAnN+o0=" },
                    //new User {UserID = 2, RoleID = 2, Created = DateTime.Now, IsActivated = true, Email = "graaf43479@ya.ru",Login = "graf434792", Password = "graf43479" },
                    //new User {UserID = 3, RoleID = 2, Created = DateTime.Now, IsActivated = true, Email = "graff43479@ya.ru",Login = "graf434793", Password = "graf43479" },
                    //new User {UserID = 4, RoleID = 1, Created = DateTime.Now, IsActivated = true, Email = "grafff43479@ya.ru",Login = "graf434794", Password = "graf43479" },
                    //new User {UserID = 5, RoleID = 1, Created = DateTime.Now, IsActivated = true, Email = "gra43479@ya.ru",Login = "graf434795", Password = "graf43479" }
                };
            users.ForEach(x => context.Users.Add(x));
            context.SaveChanges();


            var comments = new List<Comment>
                {
                    new Comment{CommentID = 1, Tittle = "Мария , школа моделей Grace Models School", CreateDate = DateTime.Now, IsAccept = true, Text = "Наша школа моделей довольно часто сотрудничает с Мариной. Начинающие модели из школы не всегда абсолютные профессионалы, но это не мешает результату работы Марины. Фото всегда получаются очень высокого уровня, и за это большое спасибо Марине) Так же хочется сказать спасибо за работу с начинающими моделями, новыми лицами, за их обучение. Твои работы нас всегда радуют!"},
                    new Comment{CommentID = 2, Tittle = "Александр и Наталья", CreateDate = DateTime.Now, IsAccept = true, Text = "Вчера мы получили волшебную коробочку с нашими фотографиями со свадьбы! Еще при встрече \"пищали\" от восторга при просмотре напечатанных фотографий Фотографии сказочные! Видна рука мастера. Ни одной упущенной детали . Колечки... декор...туфельки и все так красиво преподнесено на фото.Ты очень позитивный и чуткий человек, а главное очень улыбчивый ! Спасибо,что была с нами в этот день и запечатлела все важные для нас моменты! Мы желаем тебе удачи, красивых снимков и процветания!"},
                    new Comment{CommentID = 3, Tittle = "Алина", CreateDate = DateTime.Now, IsAccept = true, Text = "Мариночка!Спасибо огромное за такие крутые модельные тесты и вселяющие уверенность в себе слова. Вышла от тебя окрыленная и с верой в себя. Целую ,обнимаю.  :*"},
                    new Comment{CommentID = 4, Tittle = "Карасевы", CreateDate = DateTime.Now, IsAccept = true, Text = "Хотим выразить благодарность от всей нашей семьи! Не зря Вас нам так нахваливала наша подруга, которая уже была у Вас на фотосессии. Фотографии превзошли все наши ожидания. Родители были просто в восторге. Мама еле сдерживала слезы радости. Я уже по телефону Вам очень много слов благодарности сказала. Но очень хочется и другим передать информацию о том, какой Вы замечательный, добродушный человек и, безусловно, мастер своего дела. Обязательно придем к Вам снова! Семья Карасевых."},
                    new Comment{CommentID = 5, Tittle = "Анастасия", CreateDate = DateTime.Now, IsAccept = true, Text = "Безумно талантливый и замечательный фотограф, ответственно подходит к своей работе, даёт ценные и нужные советы! Очень рада, что познакомилась с таким прекрасным человеком, обращусь к Мариночке еще не раз, чего и всем советую!!!"},
                    new Comment{CommentID = 6, Tittle = "Дарья", CreateDate = DateTime.Now, IsAccept = true, Text = "Благодарна судьбе, что познакомила меня с Мариной! Это профессионал своего дела! На ее фотосессиях попадаешь в необыкновенно дружескую обстановку, где время пролетает очень быстро и незаметно! А фотографии получившиеся в итоге, шедевры! Это великолепно!"},
                    new Comment{CommentID = 7, Tittle = "Елена", CreateDate = DateTime.Now, IsAccept = true, Text = "Спасибо, Мариночке, замечательному фотографу и человечку за ее прекрасные работы!) Работать с ней одно удовольствие, были учтены все мои пожелания при съемке, и как результат - отличные работы!) Спасибо за чудесное настроение от работы в вами!)"},
                    new Comment{CommentID = 8, Tittle = "Саша и Сережа", CreateDate = DateTime.Now, IsAccept = true, Text = "Наша свадьба состоялась в первый жаркий день августа! И я не совру, если скажу, что состоялась и благодаря Марине!!! Фотографии получилось просто прекрасные! Эмоции, настроение, отношение, любовь все есть на наших фотографиях! Огромное спасибо Марине за участие в нашем самом главном дне! Мы правильно сделали, что доверились ей!"},
                    new Comment{CommentID = 9, Tittle = "Мария", CreateDate = DateTime.Now, IsAccept = true, Text = "К сожалению я еще не попала на съемку к ней, но очень сильно этого хочу. Фотографиями восхищаюсь,активно слежу за выходом ее снимков, а также за ее работами. Каждый снимок уникален и индивидуален, в каждом есть изюминка. Надеюсь на скорую встречу и на индивидуальную съемку."},
                    new Comment{CommentID = 10, Tittle = "Евгений", CreateDate = DateTime.Now, IsAccept = true, Text = "Очень хороший фотограф и просто замечательный человек! Теперь только к ней, чего и вам советую! Спасибо тебе!"},
                    new Comment{CommentID = 11, Tittle = "Наталья", CreateDate = DateTime.Now, IsAccept = true, Text = "Безумно талантливый человек. Фотографии делает просто потрясающие! Во время фотосессии царит гармония и веселье, нет никакого дискомфорта. Человека располагает к себе моментально, нет ни сжатости и стеснения. Она на столько позитивна, что ты сам заряжаешься позитивом и фотографии получаются яркие и красочные. Не устаешь ей говорить спасибо за ту красоту которую она делает на съемке. Фотографировалась у нее не один раз и каждый раз под большим впечатлением. Эмоции бьют через край. Безумно добрый и позитивный человек.Еще раз огромное спасибо за фотографии. Теперь только с тобой буду работать и советую всем! Не пожалеете!"},
                };
            comments.ForEach(x => context.Comments.Add(x));
            context.SaveChanges();




            var calendars = new List<Calendar>
                {
                    new Calendar{CalendarID = 1, CalendarDate = DateTime.Now.AddDays(15), CalendarDescription = "Фотографируем кошечек"},
                    new Calendar{CalendarID = 2, CalendarDate = DateTime.Now.AddDays(5), CalendarDescription = "Фотографируем собачек"},
                    new Calendar{CalendarID = 3, CalendarDate = DateTime.Now.AddDays(7), CalendarDescription = "Фотографируем человеков"},
                    new Calendar{CalendarID = 4, CalendarDate = DateTime.Now.AddDays(9), CalendarDescription = "Фотографируем свадьбу"},
                    new Calendar{CalendarID = 5, CalendarDate = DateTime.Now.AddDays(-10), CalendarDescription = "Фотографируем свадьбу"},
                    new Calendar{CalendarID = 6, CalendarDate = DateTime.Now.AddDays(-30), CalendarDescription = "Фотографируем кошечек"},
                    new Calendar{CalendarID = 7, CalendarDate = DateTime.Now.AddDays(45), CalendarDescription = "Фотографируем кошечек"},
                    new Calendar{CalendarID = 8, CalendarDate = DateTime.Now.AddDays(18), CalendarDescription = "Фотографируем свадьбу"},
                    new Calendar{CalendarID = 9, CalendarDate = DateTime.Now.AddDays(29), CalendarDescription = "Фотографируем собачку"},
                    new Calendar{CalendarID = 10, CalendarDate = DateTime.Now.AddDays(4), CalendarDescription = "Фотографируем свадьбу"},
                    new Calendar{CalendarID = 11, CalendarDate = DateTime.Now.AddDays(59), CalendarDescription = "Фотографируем кошечек"},
                    new Calendar{CalendarID = 12, CalendarDate = DateTime.Now.AddDays(3), CalendarDescription = "Фотографируем кошечек"},
                    new Calendar{CalendarID = 13, CalendarDate = DateTime.Now.AddDays(33), CalendarDescription = "Фотографируем собачек"},
                };
            calendars.ForEach(x => context.Calendars.Add(x));
            context.SaveChanges();
         //   base.Seed(context);
        }

        public static int DigitGnerate()
        {
            Random rnd = new Random();
            return rnd.Next(0, 20);
        }

    }
}
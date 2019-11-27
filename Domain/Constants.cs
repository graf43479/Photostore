using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Constants
    {
        //количество товаров на одной странице Product/List
        public const int PRODUCT_PAGE_SIZE = 12;

        public const string BREADCRUMBS_SEPARATOR = " > ";

        //public const string SUPER_CATEGORY_MINI_IMAGES_FOLDER = "SuperCategoryMiniImages";
        //public const int SUPER_CATEGORY_MINI_IMAGE_HEIGHT = 168;
        //public const int SUPER_CATEGORY_MINI_IMAGE_WIDTH = 238;

        //public const string CATEGORY_MINI_IMAGES_FOLDER = "CategoryMiniImages";
        //public const int CATEGORY_NUMBER_PER_ROW = 3;
        //public const int CATEGORY_MINI_IMAGE_HEIGHT = 168;
        //public const int CATEGORY_MINI_IMAGE_WIDTH = 238;
        // Папки для загрузки картинок для фотогалереи раздела Коллекции
        public const string PRODUCT_IMAGE_FOLDER = "img"; //"img";
        public const string PRODUCT_IMAGE_PREVIEW_FOLDER = "cache";//"cache";

        // Размеры уменьшенной и увеличенной картинки для фотогалереи раздела Коллекции
        //public const string PRODUCT_MINI_IMAGES_FOLDER = "ProductMiniImages";
        //public const int PRODUCT_NUMBER_PER_ROW = 3;
        //public const int PRODUCT_MINI_IMAGE_HEIGHT = 0;
        //public const int PRODUCT_MINI_IMAGE_WIDTH = 320;

        ////Галерея изображений для статей и нвосотей
        //public const string GALERY_IMAGES_FOLDER = "Galery";
        //public const int GALERY_IMAGES_HEIGHT = 0;
        //public const int GALERY_IMAGES_WIDTH = 450;


        // Размеры уменьшенной и увеличенной картинки для фотогалереи раздела Товары
        public const int PRODUCT_IMAGE_HEIGHT = 0;
        public const int PRODUCT_IMAGE_WIDTH = 1550;
        public const int PRODUCT_IMAGE_PREVIEW_HEIGHT = 0; //120; //100;
        public const int PRODUCT_IMAGE_PREVIEW_WIDTH = 450; //192;//0;
        public const int PRODUCT_IMAGE_PREVIEW_COUNT = 5;

        // Папки для загрузки картинок новостной ленты
        //public const string NEWS_MINI_IMAGES_FOLDER = "NewsMiniImages";
        //public const int NEWS_MINI_IMAGE_HEIGHT = 168;
        //public const int NEWS_MINI_IMAGE_WIDTH = 238;

        //// Папки для загрузки картинок статей
        //public const string ARTICLE_MINI_IMAGES_FOLDER = "ArticleMiniImages";
        //public const int ARTICLE_MINI_IMAGE_HEIGHT = 168;
        //public const int ARTICLE_MINI_IMAGE_WIDTH = 238;




        //Количество элементов на странице в админском блоке
        public const int ADMIN_PAGE_SIZE = 30;

        //настройки сервера электронной почты 
        public const string MAIL_TO_ADDRESS = "graf43479@ya.ru";
        public const string MAIL_FROM_ADDRESS = "graf43479@ya.ru";
        public const bool USE_SSL = true;
        public const string USERNAME = "graf43479";
        public const string PASSWORD = "votsnorov1987";
        public const string SERVERNAME = "smtp.yandex.ru";
        public const int SERVER_PORT = 587;
        public const bool WRITE_AS_FILE = false;
        public const string FILE_LOCATION = @"c:/sportstore/emails";

        //Атрибуты сайта
        public const string SITE_URL = "http://tropic-store.ru";
        public const string SITE_NAME = "tropic-store.ru";
        public const string SITE_NUMBER = "8 (968) 813-83-44";

        //% скидки на товар. Если предшествующая цена выше текущей более чем на это значение - отображается инфа о скидке 
        public const decimal PRODUCT_SALE_PERCENT = 10;

        //ID Пользователя anonymous
        public const int ANONYMOUS_ID = 14;
        public const string ANONYMOUS_LOGIN = "viktor";

        //Минимальное количество сопутствующего товара, проданного в месте с рассматриваемым товаром. 
        public const int SALE_PRODUCT_COUNT = 3;


        public static readonly List<string> RESERVED_WORDS = new List<string>()
        {
            "home","account","index","details","create","edit","delete","up","down",
            "logon","logoff","register","changepassword","changepasswordsuccess",
            "manufacturer", "order", "orders", "admin", "product", "products"
        };

        public static readonly Dictionary<char, string> TRANSLIT = new Dictionary<char, string>()
        {
            {'а',"a"}, {'б',"b"}, {'в',"v"}, {'г',"g"}, {'д',"d"}, {'е',"e"}, {'ё',"jo"}, {'ж',"z"}, {'з',"z"}, {'и',"i"}, {'й',"j"},
            {'к',"k"}, {'л',"l"}, {'м',"m"}, {'н',"n"}, {'о',"o"}, {'п',"p"}, {'р',"r"}, {'с',"s"}, {'т',"t"}, {'у',"u"}, {'ф',"f"},
            {'х',"h"}, {'ц',"c"}, {'ч',"c"}, {'ш',"s"}, {'щ',"sc"}, {'ъ',""}, {'ы',"y"}, {'ь',""}, {'э',"e"}, {'ю',"ju"}, {'я',"ja"},
            {'А',"a"}, {'Б',"b"}, {'В',"v"}, {'Г',"g"}, {'Д',"d"}, {'Е',"e"}, {'Ё',"jo"}, {'Ж',"z"}, {'З',"z"}, {'И',"i"}, {'Й',"j"},
            {'К',"k"}, {'Л',"l"}, {'М',"m"}, {'Н',"n"}, {'О',"o"}, {'П',"p"}, {'Р',"r"}, {'С',"s"}, {'Т',"t"}, {'У',"u"}, {'Ф',"f"},
            {'Х',"h"}, {'Ц',"c"}, {'Ч',"c"}, {'Ш',"s"}, {'Щ',"sc"}, {'Ъ',""}, {'Ы',"y"}, {'Ь',""}, {'Э',"e"}, {'Ю',"ju"}, {'Я',"ja"},
            {'a',"a"}, {'b',"b"}, {'c',"c"}, {'d',"d"}, {'e',"e"}, {'f',"f"}, {'g',"g"}, {'h',"h"}, {'i',"i"}, {'j',"j"}, {'k',"k"},
            {'l',"l"}, {'m',"m"}, {'n',"n"}, {'o',"o"}, {'p',"p"}, {'q',"q"}, {'r',"r"}, {'s',"s"}, {'t',"t"}, {'u',"u"}, {'v',"v"},
            {'w',"w"}, {'x',"x"}, {'y',"y"}, {'z',"z"},
            {'A',"a"}, {'B',"b"}, {'C',"c"}, {'D',"d"}, {'E',"e"}, {'F',"f"}, {'G',"g"}, {'H',"h"}, {'I',"i"}, {'J',"j"}, {'K',"k"},
            {'L',"l"}, {'M',"m"}, {'N',"n"}, {'O',"o"}, {'P',"p"}, {'Q',"q"}, {'R',"r"}, {'S',"s"}, {'T',"t"}, {'U',"u"}, {'V',"v"},
            {'W',"w"}, {'X',"x"}, {'Y',"y"}, {'Z',"z"},
            {'0',"0"}, {'1',"1"}, {'2',"2"}, {'3',"3"}, {'4',"4"}, {'5',"5"}, {'6',"6"}, {'7',"7"}, {'8',"8"}, {'9',"9"}, {'_',"_"},
            {' ',"-"}, {'-',"-"}
        };

        public static string TransliterateText(string param)
        {
            param = param.TrimEnd();
            string strResult = string.Empty;

            foreach (char ch in param)
            {
                if (Constants.TRANSLIT.Keys.Contains(ch))
                    strResult += Constants.TRANSLIT[ch];
            }

            if (Constants.RESERVED_WORDS.Contains(strResult.ToLower()))
                strResult = strResult + "exception";

            if (String.IsNullOrWhiteSpace(strResult)) strResult += "-";

            return strResult;
        }
    }
}

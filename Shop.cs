using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shop
{
    public interface IItem : ICloneable, IComparable
    {
        double price { get; set; }
        string model { get; set; }
        string manufacturer { get; set; }
        string material { get; set; }

    }
    public interface IWarehouse
    {
        Dictionary<int, IItem> catalog { get; set; }
        void showCatalog();
    }


    public class Shop1 : IEnumerable<IItem>, IList<IItem>
    {

        public enum getMemType { SIMM, DIMM, DDR, DDR2, DDR3 };

        public class Warehouse : IWarehouse
        {
            public Dictionary<int, IItem> catalog { get; set; }
            public Warehouse(List<IItem> catalog)
            {
                Random random = new Random();
                this.catalog = new Dictionary<int, IItem>();
                foreach (var item in catalog)
                {
                    this.catalog.Add(random.Next(1000000), item);
                }
            }
            public void showCatalog()
            {


                foreach (var item in catalog)
                {
                    Console.Write(item.Key.ToString() + '\t');
                    Console.WriteLine(item.Value.ToString());
                }
            }
        }




        public abstract class ItemBase : IItem
        {
            protected abstract string GetDescription();
            protected abstract object GetClone();
            public sealed override string ToString() { return GetDescription(); }// return string.Format("{0}\t{1}\t{2}\t{3}", producer, model, price, material); }
            public double price { get; set; }
            public string model { get; set; }
            public string manufacturer { get; set; }
            public string material { get; set; }

            public int CompareTo(object obj)
            {
                if (obj == null) return 1;

                IItem item = obj as IItem;
                if (item != null)
                    return this.price.CompareTo(item.price);
                else
                    throw new ArgumentException("Object is not a Temperature");
            }
            public object Clone()
            {
                return GetClone();
            }
        }

        public class Keyboard : ItemBase
        {
            public int countKey { get; set; }
            public int countTones { get; set; }
            public Keyboard(string manufacturer, string model, double price, int countKey, int countTones, string material)
            {
                this.manufacturer = manufacturer;
                this.model = model;
                this.price = price;
                this.countKey = countKey;
                this.countTones = countTones;
                this.material = material;


            }


            protected override object GetClone()
            {
                return new Keyboard(this.manufacturer, this.model, this.price, this.countKey, this.countTones, this.material);

            }

            protected override string GetDescription()
            {
                return String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", manufacturer, model, price.ToString(), ((int)countTones).ToString(), countKey.ToString(), material);

            }
        }
        public class GuitarItem : ItemBase
        {

            public getMemType countKey { get; set; }
            public string Type { get; set; }


            public GuitarItem(string manufacturer, string model, double price, getMemType countKey, string Type, string material)
            {
                this.manufacturer = manufacturer;
                this.model = model;
                this.price = price;
                this.countKey = countKey;
                this.Type = Type;
                this.material = material;


            }
            protected override string GetDescription()
            {
                return string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", manufacturer, model, price.ToString(), ((int)countKey).ToString(), Type.ToString(), material);

            }
            protected override object GetClone()
            {
                return new GuitarItem(this.manufacturer, this.model, this.price, this.countKey, this.Type, this.material);

            }


        }



        public delegate int Compare(IItem x, IItem y);
        // описание класса
        private List<IItem> catalog;
        public delegate void Finished(Shop1 sender);
        public event Finished FinishedEvent;
        public Shop1(List<IItem> catalog)
        {
            setProducts(catalog);
            FinishedEvent += Shop1_FinishedEvent;

        }

        public void Shop1_FinishedEvent(Shop1 sender)
        {
            Console.Write("Сортировка окончена.\n");
        }
        public void addProduct(IItem guitar)
        {
            catalog.Add(guitar);
        }
        public List<IItem> getProducts() { return catalog; }
        public void setProducts(List<IItem> catalog) { this.catalog = catalog; }
        public void ShowItems()
        {
            foreach (IItem guitar in catalog)
            {
                Console.WriteLine(guitar.ToString());
            }
        }


        public IEnumerator<IItem> GetEnumerator()
        {
            return (IEnumerator<IItem>)GetEnum();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public CatalogEnum GetEnum()
        {
            return new CatalogEnum(catalog);
        }
        public class CatalogEnum : IEnumerator<IItem>
        {
            public List<IItem> _catalog;
            int position = -1;

            public CatalogEnum(List<IItem> list)
            {
                _catalog = list;
            }

            public bool MoveNext()
            {
                position++;
                return (position < _catalog.Count);
            }

            public void Reset()
            {
                position = -1;
            }
            public void Dispose()
            {

            }


            public IItem Current
            {
                get
                {
                    try
                    {
                        return _catalog[position];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }


        }


        public IItem this[int index] { get { return catalog[index]; } set { catalog[index] = value; } }
        public int IndexOf(IItem item)
        {
            int i = 0;
            foreach (var it in catalog)
            {
                if (it == item)
                {
                    return i;
                }
                i++;
            }
            return -1;
        }
        public void RemoveAt(int index)
        {
            try
            {
                var cat = new List<IItem>();
                for (int i = index; i < catalog.Count - 1; i++)
                {
                    if (index != i)
                        cat.Add(catalog[i]);

                }
                catalog = cat;
            }
            catch (Exception)
            { throw new System.ArgumentOutOfRangeException(); }
        }
        public void Insert(int index, IItem it)
        {
            try
            {
                catalog[index] = it;
            }
            catch (Exception)
            { throw new System.ArgumentOutOfRangeException(); }
        }
        public bool Remove(IItem it)
        {
            try
            {
                catalog.Remove(it);
                return true;
            }
            catch (Exception)
            { return false; }
        }
        public bool IsReadOnly
        {
            get { return false; }
        }
        public int Count
        {
            get { return catalog.Count; }
        }
        public void Add(IItem it)
        {
            catalog.Add(it);
        }
        public void Clear()
        {
            catalog.Clear();
        }
        public bool Contains(IItem it)
        {
            return catalog.Contains(it);
        }
        public void CopyTo(IItem[] ar, int index)
        {
            catalog.CopyTo(ar, index);
        }
        //методы для сортировки полей
        public int comparePrice(IItem X, IItem Y)
        {
            return X.CompareTo(Y);
        }
        public int compareModel(IItem X, IItem Y)
        {
            return String.Compare(X.model, Y.model);
        }
        public int comparemanufacturer(IItem X, IItem Y)
        {
            return String.Compare(X.manufacturer, Y.manufacturer);
        }


        public int compareMaterial(IItem X, IItem Y)
        {
            return String.Compare(X.material, Y.material);
        }


        public void Sort(Compare compare, bool descending)
        {
            if (!descending)
            {
                for (int i = 0; i < catalog.Count; i++)
                {
                    for (int j = 0; j < catalog.Count - 1; j++)
                    {
                        if (compare(catalog[j], catalog[j + 1]) == 1)
                        {

                            IItem guitar = catalog[j];
                            catalog[j] = catalog[j + 1];
                            catalog[j + 1] = guitar;
                        }


                    }
                }
            }
            else
            {
                for (int i = 0; i < catalog.Count; i++)
                {
                    for (int j = 0; j < catalog.Count - 1; j++)
                    {
                        if (compare(catalog[j], catalog[j + 1]) == -1)
                        {

                            IItem guitar = catalog[j];
                            catalog[j] = catalog[j + 1];
                            catalog[j + 1] = guitar;
                        }


                    }
                }
            }
            FinishedEvent(this);

        }
    }
}
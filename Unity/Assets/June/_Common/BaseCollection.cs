using System;
using System.Collections;
using System.Collections.Generic;
using June.Core;


/// <summary>
/// Base collection.
/// </summary>
public class BaseCollection<T> : IEnumerable<T> where T : BaseModel
{
	
	private SimpleJson.JsonArray _Collection;

	private Func<IDictionary<string, object>, T> _ItemCtor;

	protected T[] _Items;

	/// <summary>
	/// Gets the <see cref="`1"/> at the specified index.
	/// </summary>
	/// <value>
	/// The <see cref="`1"/>.
	/// </value>
	/// <param name="index">The index.</param>
	/// <returns></returns>
	public T this [int index] {
		get {
			return _Items [index];
		}
	}

	/// <summary>
	/// Gets the count.
	/// </summary>
	/// <value>
	/// The count.
	/// </value>
	public int Count {
		get {
			return null == _Items ? 0 : _Items.Length;
		}
	}

	public int IndexOf (T item)
	{
		return null == _Items ? 0 : Array.IndexOf (_Items, item);
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseCollection`1"/> class.
	/// </summary>
	/// <param name="itemArray">Item array.</param>
	/// <param name="constructor">Constructor.</param>
	public BaseCollection (SimpleJson.JsonArray itemArray, Func<IDictionary<string, object>, T> constructor)
	{
		_Collection = itemArray;
		_ItemCtor = constructor;
		UpdateRaw (itemArray);
	}

	/// <summary>
	/// Updates the raw record.
	/// </summary>
	/// <param name="itemArray">Item array.</param>
	public void UpdateRaw (SimpleJson.JsonArray itemArray)
	{
		_Collection = itemArray;
		if (null != itemArray) {
			_Items = new T[itemArray.Count];
			
			for (int i = 0; i < itemArray.Count; i++) {
				_Items [i] = _ItemCtor ((IDictionary<string, object>)itemArray [i]);
			}
		}
	}

	#region IEnumerable implementation

	/// <summary>
	/// Gets the generic enumerator.
	/// </summary>
	/// <returns>The enumerator.</returns>
	public IEnumerator<T> GetEnumerator ()
	{
		foreach (T item in _Items) {
			yield return item;
		}
	}

	/// <summary>
	/// Gets the enumerator.
	/// </summary>
	/// <returns>The enumerator.</returns>
	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
	{
		return this.GetEnumerator ();
	}

	#endregion

	/// <summary>
	/// Add the specified item.
	/// *** NOTE: this does NOT update the underlying record.
	/// </summary>
	/// <param name="item">Item.</param>
	public void Add (T item)
	{
		Array.Resize (ref _Items, _Items.Length + 1);
		_Items [_Items.Length - 1] = item;
	}

	/// <summary>
	/// Add the specified item.
	/// </summary>
	/// <param name="item">Item.</param>
	public void Add (IDictionary<string, object> item)
	{
		_Collection.Add (item);
		Array.Resize (ref _Items, _Items.Length + 1);
		_Items [_Items.Length - 1] = _ItemCtor (item);
	}

	/// <summary>
	/// Remove the specified item.
	/// </summary>
	/// <param name="item">Item.</param>
	public void Remove (T item)
	{
		if (null != _Items && _Items.Length > 0) {
			T[] updatedItems = new T[_Items.Length - 1];
			int j = 0;
			for (int i = 0; i < _Items.Length; i++) {
				if (_Items [i] != item) {
					updatedItems [j++] = _Items [i];
				}
			}
			_Items = updatedItems;
		}
	}

	/// <summary>
	/// Gets the raw object.
	/// </summary>
	/// <returns>The raw.</returns>
	public SimpleJson.JsonArray GetRaw ()
	{
		return _Collection;
	}

	/// <summary>
	/// Returns a <see cref="System.String"/> that represents the current <see cref="BaseCollection`1"/>.
	/// </summary>
	/// <returns>A <see cref="System.String"/> that represents the current <see cref="BaseCollection`1"/>.</returns>
	public override string ToString ()
	{
		return string.Format ("[Collection Count:{0}\n{1}]", 
			Count,
			(Count > 0) 
		                      	? string.Join ("\n\t", Array.ConvertAll (_Items, i => i.ToString ()))
		                      	: "<empty>");
	}
}

/// <summary>
/// Base collection sorted.
/// </summary>
public class BaseCollectionSorted<T> : BaseCollection<T> where T : BaseModel
{

	protected Comparison<T> _SortFunction;

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseCollectionSorted`1"/> class.
	/// </summary>
	/// <param name="itemArray">Item array.</param>
	/// <param name="constructor">Constructor.</param>
	/// <param name="sortFunction">Sort function.</param>
	public BaseCollectionSorted (SimpleJson.JsonArray itemArray, Func<IDictionary<string, object>, T> constructor, Comparison<T> sortFunction) : base (itemArray, constructor)
	{
		_SortFunction = sortFunction;
		Sort ();
		//flagInt ();
	}

	public virtual void Sort ()
	{
		if (null != _Items && null != _SortFunction) {
			Array.Sort (_Items, _SortFunction);
		}
	}
}

//public partial class BaseList<T> : IList<T> where T : BaseModel {
//
//	List<object> _Records = null;
//
//	#region IList implementation
//	public int IndexOf (T item) {
//		return _Records.IndexOf((object)item);
//	}
//
//	public void Insert (int index, T item) {
//		_Records.Insert(index, item);
//	}
//
//	public void RemoveAt (int index) {
//		_Records.RemoveAt(index);
//	}
//
//	public T this [int index] {
//		get {
//			return (T)_Records[index];
//		}
//		set {
//			_Records[index] = value;
//		}
//	}
//	#endregion
//	#region ICollection implementation
//	public void Add (T item) {
//		_Records.Add(item);
//	}
//
//	public void Clear () {
//		_Records.Clear();
//	}
//
//	public bool Contains (T item) {
//		return _Records.Contains(item);
//	}
//
//	public void CopyTo (T[] array, int arrayIndex) {
//		// NOT IMPLEMENTED
//	}
//
//	public bool Remove (T item) {
//		return _Records.Remove(item);
//	}
//
//	public int Count {
//		get {
//			return _Records.Count;
//		}
//	}
//
//	public bool IsReadOnly {
//		get {
//			return false;
//		}
//	}
//	#endregion
//	#region IEnumerable implementation
//	public IEnumerator<T> GetEnumerator () {
//		foreach(var item in _Records) {
//			yield return (T)item;
//		}
//	}
//	#endregion
//	#region IEnumerable implementation
//	IEnumerator IEnumerable.GetEnumerator ()
//	{
//		return this.GetEnumerator();
//	}
//	#endregion
//
//	#region Ctors
//
//	public BaseList(List<object> records) {
//		_Records = records;
//	}
//
//	public BaseList(ICollection<T> values) : this(new List<T>(values)) { }
//
//	public BaseList(IEnumerable<T> values) : this(new List<T>(values)) { }
//
//	public BaseList() : this(new List<object>()) { }
//
//	#endregion
//}
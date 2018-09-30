/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

using System.Collections;
using System.Collections.Generic;

namespace CjLib
{
  public class CollectionUtil
  {
    public struct Pair<T>
    {
      public T Prev;
      public T Curr;
      public Pair(T prev, T curr)
      {
        Prev = prev;
        Curr = curr;
      }
    }

    public static IEnumerable<Pair<T>> Pairs<T>(IEnumerable<T> enumerable)
    {
      return new PairEnumerable<T>(enumerable);
    }

    private class PairEnumerable<T> : IEnumerable<Pair<T>>
    {
      public IEnumerable<T> m_enumerable;
      public PairEnumerable(IEnumerable<T> enumerable)
      {
        m_enumerable = enumerable;
      }

      public IEnumerator<Pair<T>> GetEnumerator()
      {
        IEnumerator<T> itPrev = m_enumerable.GetEnumerator();
        IEnumerator<T> itCurr = m_enumerable.GetEnumerator();
        itPrev.MoveNext();
        itCurr.MoveNext();
        while (itCurr.MoveNext())
        {
          yield return new Pair<T>(itPrev.Current, itCurr.Current);
          itPrev.MoveNext();
        }
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
        return GetEnumerator();
      }
    }
  }
}

using System;
using System.Collections.Generic;

namespace PatternMatch
{	
	internal class Matcher<T>
	{
		private Func<T> accessor;
		
		private List<KeyValuePair<Func<T,bool>,Action<T>>> matchers = new List<KeyValuePair<Func<T, bool>, Action<T>>>();
		
		private Action<T> elseContinuation;
		
		public Matcher(Func<T> accessor)
		{
			if(accessor == null)
				throw new ArgumentNullException("Accessor cannot be null!");
			this.accessor = accessor;
		}
		
		public static Matcher<T> Match(Func<T> accessor)
		{
			return new Matcher<T>(accessor);
		}
		
		public static Matcher<T> Match(T literal)
		{
			return new Matcher<T>(delegate() {return literal;});
		}
		
		public Matcher<T> With(Func<T,bool> predicate,Action<T> continuation)
		{
			if(predicate == null || continuation == null)
				throw new ArgumentNullException("Pattern must have a predicate and a continuation");
			this.matchers.Add(new KeyValuePair<Func<T,bool>,Action<T>>(predicate,continuation));
			return this;
		}
		
		public Matcher<T> With(T literal,Action<T> continuation)
		{
			if(literal == null || continuation == null)
				throw new ArgumentNullException("Pattern must have a predicate and a continuation");
			this.matchers.Add(new KeyValuePair<Func<T,bool>,Action<T>>(delegate(T val){return val.Equals(literal);},continuation));
			return this;
		}
		
		public Matcher<T> Else(Action<T> continuation)
		{
			this.elseContinuation = continuation;
			return this;
		}
		
		public void Go()
		{
			T val = this.accessor();
			foreach(KeyValuePair<Func<T,bool>,Action<T>> matcher in this.matchers)
			{
				if(matcher.Key(val))
				{
					matcher.Value(val);
					return;
				}
			}
			
			if(this.elseContinuation != null)
			{
				this.elseContinuation(val);
			}
			else
			{
				throw new Exception("No patterns matched the value!");	
			}
		}
	}
	
	internal class Matcher<T,TReturn>
	{
		private Func<T> accessor;
		
		private List<KeyValuePair<Func<T,bool>,Func<T,TReturn>>> matchers = new List<KeyValuePair<Func<T, bool>, Func<T,TReturn>>>();
		
		private Func<T,TReturn> elseContinuation;
		
		public Matcher(Func<T> accessor)
		{
			if(accessor == null)
				throw new ArgumentNullException("Accessor cannot be null!");
			this.accessor = accessor;
		}
		
		public static Matcher<T,TReturn> Match(Func<T> accessor)
		{
			return new Matcher<T,TReturn>(accessor);
		}
		
		public static Matcher<T,TReturn> Match(T literal)
		{
			return new Matcher<T,TReturn>(delegate() {return literal;});
		}
		
		public Matcher<T,TReturn> With(Func<T,bool> predicate,Func<T,TReturn> continuation)
		{
			if(predicate == null || continuation == null)
				throw new ArgumentNullException("Pattern must have a predicate and a continuation");
			this.matchers.Add(new KeyValuePair<Func<T,bool>,Func<T,TReturn>>(predicate,continuation));
			return this;
		}
		
		public Matcher<T,TReturn> With(T literal,Func<T,TReturn> continuation)
		{
			if(literal == null || continuation == null)
				throw new ArgumentNullException("Pattern must have a predicate and a continuation");
			this.matchers.Add(new KeyValuePair<Func<T,bool>,Func<T,TReturn>>(delegate(T val){return val.Equals(literal);},continuation));
			return this;
		}
		
		public Matcher<T,TReturn> Else(Func<T,TReturn> continuation)
		{
			this.elseContinuation = continuation;
			return this;
		}
		
		public TReturn Go()
		{
			T val = this.accessor();
			foreach(KeyValuePair<Func<T,bool>,Func<T,TReturn>> matcher in this.matchers)
			{
				if(matcher.Key(val))
				{
					return matcher.Value(val);
				}
			}
			
			if(this.elseContinuation != null)
			{
				return this.elseContinuation(val);
			}
			else
			{
				throw new Exception("No patterns matched the value!");	
			}
		}
	}
}


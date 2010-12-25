using System;
using NUnit.Framework;
using PatternMatch;
namespace PatternMatch.Tests
{
	[TestFixture()]
	public class Test
	{
		[Test()]
		public void ClosureContinuation()
		{
			bool five = false;
			Matcher<int>.Match(delegate() {return 5;}).With(x => x == 5,x => five = true).Else(x => System.Console.WriteLine("Not 5!")).Go();
			Assert.AreEqual(true, five);
		}
		
		[Test()]
		public void LiteralValue()
		{
			bool five = false;
			Matcher<int>.Match(5).With(x => x == 5,x => five = true).Else(x => System.Console.WriteLine("Not 5!")).Go();
			Assert.AreEqual(true, five);
		}
		
		[Test()]
		public void LiteralValueAndMatcher()
		{
			bool five = false;
			Matcher<int>.Match(5).With(5,x => five = true).Else(x => System.Console.WriteLine("Not 5!")).Go();
			Assert.AreEqual(true, five);
		}
		
		[Test()]
		public void ElseContinuation()
		{
			bool five = false;
			Matcher<int>.Match(delegate() {return 6;}).With(x => x == 5,x => five = true).Else(x => System.Console.WriteLine("Not 5!")).Go();
			Assert.AreNotEqual(true, five);
		}
		
		[Test()]
		[ExpectedExceptionAttribute(typeof(Exception))]
		public void ThrowsWithIncompletePattern()
		{
			bool five = false;
			Matcher<int>.Match(delegate() {return 6;}).With(x => x == 5,x => five = true).Go();	
		}
		
		[Test()]
		public void Order()
		{
			//patterns are matched in order, first pattern wins
			bool fiveOrSix = false;
			bool justSix = false;
			Matcher<int>.Match(delegate() {return 6;}).With(x => x == 5 || x == 6,x => fiveOrSix = true).With(x=>x==6,x=>justSix = true).Go();
			Assert.AreEqual(true, fiveOrSix);    
			Assert.AreEqual(false, justSix);  
		}
		
		
		[Test()]
		public void ReturnsWithDelegate()
		{
			Assert.IsTrue(Matcher<int,bool>.Match(delegate() {return 5;}).With(x => x == 5,delegate(int x){return true;}).Else(delegate(int x){return false;}).Go());
		}
		
		[Test()]
		public void ReturnsWithLambda()
		{
			Assert.IsTrue(Matcher<int,bool>.Match(delegate() {return 5;}).With(x => x == 5,x => true).Else(x=>false).Go());
		}
		
		[Test()]
		public void ReturnsElse()
		{
			Assert.IsFalse(Matcher<int,bool>.Match(delegate() {return 6;}).With(x => x == 5,delegate(int x){return true;}).Else(delegate(int x){return false;}).Go());
		}
		
		[Test()]
		public void ReturnsElseLambda()
		{
			Assert.IsFalse(Matcher<int,bool>.Match(delegate() {return 6;}).With(x => x == 5,x => true).Else(x=>false).Go());
		}
		
		[Test()]
		public void Recursion()
		{
			Assert.AreEqual(0,RecursiveFunction(5));	
		}
		
		public int RecursiveFunction(int y)
		{
			return Matcher<int,int>.Match(delegate() {return y;}).With(x => x > 0,x => RecursiveFunction(x - 1)).Else(x=>x).Go();
		}
		
		public int RecursiveFunction1(int y)
		{
			return Matcher<int,int>
				.Match(y)
				.With(x=> x > 0, x=> RecursiveFunction1(x - 1))
				.Else(x=> x)
				.Go();
		}
	}
}


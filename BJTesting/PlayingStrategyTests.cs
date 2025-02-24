﻿using BJ;
using BJ.PlayingStrategies;

namespace BJTesting
{
	[TestClass]
	public class PlayingStrategyTests
	{
		private static readonly PlayingStrategy BASICSTRATEGY_2D = new PlayingStrategyDD();
		private static readonly PlayingStrategy BASICSTRATEGY_2D_WITHDEVIATIONS = new PlayingStrategyDD().UseDeviations();
		private static readonly PlayingStrategy BASICSTRATEGY_4D = new PlayingStrategyShoe();
		private static readonly PlayingStrategy DEALERSTRATEGY_H17 = new DealerStrategy_H17();
		private static readonly PlayingStrategy DEALERSTRATEGY_S17 = new DealerStrategy_S17();

		[TestMethod]
		public void BasicStrategy_IsCorrectForFreshHardHands()
		{
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 4, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 5, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 6, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 7, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 8, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 9, [["2", "3", "4", "5", "6"], ["7", "8", "9", "10", "J", "Q", "K", "A"]], [ActionEnum.D, ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 10, [["2", "3", "4", "5", "6", "7", "8", "9"], ["10", "J", "Q", "K", "A"]], [ActionEnum.D, ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 11, [Card.RANKS], [ActionEnum.D]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 12, [["2", "3", "7", "8", "9", "10", "J", "Q", "K", "A"], ["4", "5", "6"]], [ActionEnum.H, ActionEnum.S]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 13, [["2", "3", "4", "5", "6"], ["7", "8", "9", "10", "J", "Q", "K", "A"]], [ActionEnum.S, ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 14, [["2", "3", "4", "5", "6"], ["7", "8", "9", "10", "J", "Q", "K", "A"]], [ActionEnum.S, ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 15, [["2", "3", "4", "5", "6"], ["7", "8", "9"], ["10", "J", "Q", "K", "A"]], [ActionEnum.S, ActionEnum.H, ActionEnum.R]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 16, [["2", "3", "4", "5", "6"], ["7", "8", "9"], ["10", "J", "Q", "K", "A"]], [ActionEnum.S, ActionEnum.H, ActionEnum.R]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 17, [["2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"], ["A"]], [ActionEnum.S, ActionEnum.R]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 18, [Card.RANKS], [ActionEnum.S]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 19, [Card.RANKS], [ActionEnum.S]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 20, [Card.RANKS], [ActionEnum.S]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 21, [Card.RANKS], [ActionEnum.S]));
		}

		private bool TestRowResult(PlayingStrategy strategy, int handvalue, IEnumerable<string>[] dealerupcards, ActionEnum[] expectedaction, bool issoft = false, bool cansplit = false, bool issurrenderallowed = true, bool isnew = true, int tc = 0, int hits = 0)
		{
			var hand = new TestHand(handvalue)
			{
				IsSoft = issoft,
				IsNewHand = isnew,
				Hits = hits,
				CanSplit = cansplit,
			};

			return TestRowResult(strategy, hand, dealerupcards, expectedaction, issurrenderallowed, tc);
		}

		private bool TestRowResult(PlayingStrategy strategy, IHand hand, IEnumerable<string>[] dealerupcards, ActionEnum[] expectedaction, bool issurrenderallowed = true, int tc = 0)
		{
			GameState gamestate = new()
			{
				PlayerHand = hand,
				Rules = new BlackjackRules(2, 70) { IsSurrenderAllowed = issurrenderallowed },
				TrueCount = tc
			};
			return TestRowResult(strategy, gamestate, dealerupcards, expectedaction);
		}

		private bool TestRowResult(PlayingStrategy strategy, GameState gamestate, IEnumerable<string>[] dealerupcards, ActionEnum[] expectedaction)
		{
			for (int i = 0; i < dealerupcards.Length; i++)
			{
				foreach (var rank in dealerupcards[i])
				{
					gamestate.DealerUpcard = new Card(rank);
					var actualaction = strategy.GetAction(gamestate);
					if (expectedaction[i] != actualaction)
						return false;
				}
			}
			return true;
		}

		[TestMethod]
		public void BasicStrategy_IsCorrectForFreshSoftHands()
		{
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 13, [["2", "3", "4", "7", "8", "9", "10", "J", "Q", "K", "A"], ["5", "6"]], [ActionEnum.H, ActionEnum.D], issoft: true));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 14, [["2", "3", "7", "8", "9", "10", "J", "Q", "K", "A"], ["4", "5", "6"]], [ActionEnum.H, ActionEnum.D], issoft: true));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 15, [["2", "3", "7", "8", "9", "10", "J", "Q", "K", "A"], ["4", "5", "6"]], [ActionEnum.H, ActionEnum.D], issoft: true));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 16, [["2", "3", "7", "8", "9", "10", "J", "Q", "K", "A"], ["4", "5", "6"]], [ActionEnum.H, ActionEnum.D], issoft: true));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 17, [["2", "7", "8", "9", "10", "J", "Q", "K", "A"], ["3", "4", "5", "6"]], [ActionEnum.H, ActionEnum.D], issoft: true));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 18, [["2", "3", "4", "5", "6"], ["7", "8"], ["9", "10", "J", "Q", "K", "A"]], [ActionEnum.D, ActionEnum.S, ActionEnum.H], issoft: true));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 19, [["2", "3", "4", "5", "7", "8", "9", "10", "J", "Q", "K", "A"], ["6"]], [ActionEnum.S, ActionEnum.D], issoft: true));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 20, [Card.RANKS], [ActionEnum.S], issoft: true));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 21, [Card.RANKS], [ActionEnum.S], issoft: true));
		}

		[TestMethod]
		public void BasicStrategy_IsCorrectForHitSoftHands()
		{
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 13, [Card.RANKS], [ActionEnum.H], issoft: true, isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 14, [Card.RANKS], [ActionEnum.H], issoft: true, isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 15, [Card.RANKS], [ActionEnum.H], issoft: true, isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 16, [Card.RANKS], [ActionEnum.H], issoft: true, isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 17, [Card.RANKS], [ActionEnum.H], issoft: true, isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 18, [["2", "3", "4", "5", "6", "7", "8"], ["9", "10", "J", "Q", "K", "A"]], [ActionEnum.S, ActionEnum.H], issoft: true, isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 19, [Card.RANKS], [ActionEnum.S], issoft: true, isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 20, [Card.RANKS], [ActionEnum.S], issoft: true, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 21, [Card.RANKS], [ActionEnum.S], issoft: true, hits: 1));
		}

		[TestMethod]
		public void BasicStrategy_IsCorrectForHitHardHands()
		{
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 4, [Card.RANKS], [ActionEnum.H], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 5, [Card.RANKS], [ActionEnum.H], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 6, [Card.RANKS], [ActionEnum.H], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 7, [Card.RANKS], [ActionEnum.H], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 8, [Card.RANKS], [ActionEnum.H], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 9, [Card.RANKS], [ActionEnum.H], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 10, [Card.RANKS], [ActionEnum.H], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 11, [Card.RANKS], [ActionEnum.H], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 12, [["2", "3", "7", "8", "9", "10", "J", "Q", "K", "A"], ["4", "5", "6"]], [ActionEnum.H, ActionEnum.S], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 13, [["2", "3", "4", "5", "6"], ["7", "8", "9", "10", "J", "Q", "K", "A"]], [ActionEnum.S, ActionEnum.H], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 14, [["2", "3", "4", "5", "6"], ["7", "8", "9", "10", "J", "Q", "K", "A"]], [ActionEnum.S, ActionEnum.H], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 15, [["2", "3", "4", "5", "6"], ["7", "8", "9", "10", "J", "Q", "K", "A"], ["10", "J", "Q", "K", "A"]], [ActionEnum.S, ActionEnum.H, ActionEnum.H], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 16, [["2", "3", "4", "5", "6"], ["7", "8", "9"], ["10", "J", "Q", "K", "A"]], [ActionEnum.S, ActionEnum.H, ActionEnum.H], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 17, [["2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"], ["A"]], [ActionEnum.S, ActionEnum.S], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 18, [Card.RANKS], [ActionEnum.S], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 19, [Card.RANKS], [ActionEnum.S], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 20, [Card.RANKS], [ActionEnum.S], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, 21, [Card.RANKS], [ActionEnum.S], isnew: false, hits: 1));
		}

		[TestMethod]
		public void BasicStrategy_IsCorrectForSplits()
		{
			var rules = new BlackjackRules(2, 70)
			{
				IsSurrenderAllowed = true,
				CanResplitAces = true,
			};

			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, new TestHand(4) { Cards = [new("2"), new("2")], CanSplit = true, IsNewHand = true }, [["2", "3", "4", "5", "6", "7"], ["8", "9", "10", "J", "Q", "K", "A"]], [ActionEnum.P, ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, new TestHand(6) { Cards = [new("3"), new("3")], CanSplit = true, IsNewHand = true }, [["2", "3", "4", "5", "6", "7"], ["8", "9", "10", "J", "Q", "K", "A"]], [ActionEnum.P, ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, new TestHand(8) { Cards = [new("4"), new("4")], CanSplit = true, IsNewHand = true }, [["2", "3", "4"], ["5", "6"], ["7", "8", "9", "10", "J", "Q", "K", "A"]], [ActionEnum.H, ActionEnum.P, ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, new TestHand(10) { Cards = [new("5"), new("5")], CanSplit = true, IsNewHand = true }, [["2", "3", "4", "5", "6", "7", "8", "9"], ["10", "J", "Q", "K", "A"]], [ActionEnum.D, ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, new TestHand(12) { Cards = [new("6"), new("6")], CanSplit = true, IsNewHand = true }, [["2", "3", "4", "5", "6", "7"], ["8", "9", "10", "J", "Q", "K", "A"]], [ActionEnum.P, ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, new TestHand(14) { Cards = [new("7"), new("7")], CanSplit = true, IsNewHand = true }, [["2", "3", "4", "5", "6", "7", "8"], ["9", "10", "J", "Q", "K", "A"]], [ActionEnum.P, ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, new TestHand(16) { Cards = [new("8"), new("8")], CanSplit = true, IsNewHand = true }, [["2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"], ["A"]], [ActionEnum.P, ActionEnum.R]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, new TestHand(18) { Cards = [new("9"), new("9")], CanSplit = true, IsNewHand = true }, [["2", "3", "4", "5", "6", "8", "9"], ["7", "10", "J", "Q", "K", "A"]], [ActionEnum.P, ActionEnum.S]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, new TestHand(20) { Cards = [new("10"), new("10")], CanSplit = true, IsNewHand = true }, [Card.RANKS], [ActionEnum.S]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D, new GameState { Rules = rules, PlayerHand = new TestHand(12) { Cards = [new("A"), new("A")], CanSplit = true, IsNewHand = true } }, [Card.RANKS], [ActionEnum.P]));
		}

		[TestMethod]
		public void DeviatedStrategy_IsCorrectForHardHand()
		{
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D_WITHDEVIATIONS, 4, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D_WITHDEVIATIONS, 5, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D_WITHDEVIATIONS, 6, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D_WITHDEVIATIONS, 7, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D_WITHDEVIATIONS, 8, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D_WITHDEVIATIONS, 9, [["2", "3", "4", "5", "6", "7"], ["8", "9", "10", "J", "Q", "K", "A"]], [ActionEnum.D, ActionEnum.H], tc: 3));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D_WITHDEVIATIONS, 10, [["2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A"]], [ActionEnum.D, ActionEnum.H], tc: 4));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D_WITHDEVIATIONS, 11, [Card.RANKS], [ActionEnum.D], tc: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D_WITHDEVIATIONS, 12, [["2", "3", "4", "5", "6"], ["7", "8", "9", "10", "J", "Q", "K", "A"]], [ActionEnum.S, ActionEnum.H], tc: 3));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D_WITHDEVIATIONS, new GameState { TrueCount = -2, RunningCount = -2, PlayerHand = new TestHand(12), Rules = new BlackjackRules(2, 70) { IsSurrenderAllowed = false } }, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D_WITHDEVIATIONS, 13, [["4", "5", "6"], ["2", "3", "7", "8", "9", "10", "J", "Q", "K", "A"]], [ActionEnum.S, ActionEnum.H], tc: -2));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D_WITHDEVIATIONS, 14, [["2", "3", "4", "5", "6"], ["7", "8", "9", "10", "J", "Q", "K", "A"]], [ActionEnum.S, ActionEnum.H], tc: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D_WITHDEVIATIONS, 15, [["2", "3", "4", "5", "6", "10", "J", "Q", "K"], ["7", "8", "9", "A"]], [ActionEnum.S, ActionEnum.H], tc: 4, issurrenderallowed: false));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D_WITHDEVIATIONS, new GameState { TrueCount = 5, RunningCount = 5, PlayerHand = new TestHand(16), Rules = new BlackjackRules(2, 70) { IsSurrenderAllowed = false } }, [["2", "3", "4", "5", "6", "9", "10", "J", "Q", "K"], ["7", "8", "A"]], [ActionEnum.S, ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D_WITHDEVIATIONS, 17, [["2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"], ["A"]], [ActionEnum.S, ActionEnum.R]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D_WITHDEVIATIONS, 18, [Card.RANKS], [ActionEnum.S]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D_WITHDEVIATIONS, 19, [Card.RANKS], [ActionEnum.S]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D_WITHDEVIATIONS, 20, [Card.RANKS], [ActionEnum.S]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D_WITHDEVIATIONS, 21, [Card.RANKS], [ActionEnum.S]));
		}

		[TestMethod]
		public void DeviatedStrategy_IsCorrectForPairs()
		{
			var rules = new BlackjackRules(2, 70)
			{
				IsSurrenderAllowed = true,
				CanResplitAces = true,
			};

			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D_WITHDEVIATIONS, new TestHand(4) { Cards = [new("2"), new("2")], CanSplit = true, IsNewHand = true }, [["2", "3", "4", "5", "6", "7"], ["8", "9", "10", "J", "Q", "K", "A"]], [ActionEnum.P, ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D_WITHDEVIATIONS, new TestHand(6) { Cards = [new("3"), new("3")], CanSplit = true, IsNewHand = true }, [["2", "3", "4", "5", "6", "7"], ["8", "9", "10", "J", "Q", "K", "A"]], [ActionEnum.P, ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D_WITHDEVIATIONS, new TestHand(8) { Cards = [new("4"), new("4")], CanSplit = true, IsNewHand = true }, [["2", "3", "4"], ["5", "6"], ["7", "8", "9", "10", "J", "Q", "K", "A"]], [ActionEnum.H, ActionEnum.P, ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D_WITHDEVIATIONS, new TestHand(10) { Cards = [new("5"), new("5")], CanSplit = true, IsNewHand = true }, [["2", "3", "4", "5", "6", "7", "8", "9"], ["10", "J", "Q", "K", "A"]], [ActionEnum.D, ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D_WITHDEVIATIONS, new TestHand(12) { Cards = [new("6"), new("6")], CanSplit = true, IsNewHand = true }, [["2", "3", "4", "5", "6", "7"], ["8", "9", "10", "J", "Q", "K", "A"]], [ActionEnum.P, ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D_WITHDEVIATIONS, new TestHand(14) { Cards = [new("7"), new("7")], CanSplit = true, IsNewHand = true }, [["2", "3", "4", "5", "6", "7", "8"], ["9", "10", "J", "Q", "K", "A"]], [ActionEnum.P, ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D_WITHDEVIATIONS, new TestHand(16) { Cards = [new("8"), new("8")], CanSplit = true, IsNewHand = true }, [["2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"], ["A"]], [ActionEnum.P, ActionEnum.R]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D_WITHDEVIATIONS, new TestHand(18) { Cards = [new("9"), new("9")], CanSplit = true, IsNewHand = true }, [["2", "3", "4", "5", "6", "8", "9"], ["7", "10", "J", "Q", "K", "A"]], [ActionEnum.P, ActionEnum.S]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D_WITHDEVIATIONS, new TestHand(20) { Cards = [new("10"), new("10")], CanSplit = true, IsNewHand = true }, [["2", "3", "4", "5", "7", "8", "9", "10", "A"], ["6"]], [ActionEnum.S, ActionEnum.P], tc: 4));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D_WITHDEVIATIONS, new TestHand(20) { Cards = [new("10"), new("10")], CanSplit = true, IsNewHand = true }, [["2", "3", "4", "7", "8", "9", "10", "A"], ["5", "6"]], [ActionEnum.S, ActionEnum.P], tc: 5));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_2D_WITHDEVIATIONS, new GameState { Rules = rules, PlayerHand = new TestHand(12) { Cards = [new("A"), new("A")], CanSplit = true, IsNewHand = true } }, [Card.RANKS], [ActionEnum.P]));
		}

		[TestMethod]
		public void BasicStrategy_IsCorrectFor4D_FreshHardHand()
		{
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 4, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 5, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 6, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 7, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 8, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 9, [["3", "4", "5", "6"], ["2", "7", "8", "9", "10", "J", "Q", "K", "A"]], [ActionEnum.D, ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 10, [["2", "3", "4", "5", "6", "7", "8", "9"], ["10", "J", "Q", "K", "A"]], [ActionEnum.D, ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 11, [Card.RANKS], [ActionEnum.D]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 12, [["2", "3", "7", "8", "9", "10", "J", "Q", "K", "A"], ["4", "5", "6"]], [ActionEnum.H, ActionEnum.S]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 13, [["2", "3", "4", "5", "6"], ["7", "8", "9", "10", "J", "Q", "K", "A"]], [ActionEnum.S, ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 14, [["2", "3", "4", "5", "6"], ["7", "8", "9", "10", "J", "Q", "K", "A"]], [ActionEnum.S, ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 15, [["2", "3", "4", "5", "6"], ["7", "8", "9"], ["10", "J", "Q", "K", "A"]], [ActionEnum.S, ActionEnum.H, ActionEnum.R]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 16, [["2", "3", "4", "5", "6"], ["7", "8"], ["9", "10", "J", "Q", "K", "A"]], [ActionEnum.S, ActionEnum.H, ActionEnum.R]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 17, [["2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"], ["A"]], [ActionEnum.S, ActionEnum.R]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 18, [Card.RANKS], [ActionEnum.S]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 19, [Card.RANKS], [ActionEnum.S]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 20, [Card.RANKS], [ActionEnum.S]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 21, [Card.RANKS], [ActionEnum.S]));
		}

		[TestMethod]
		public void BasicStrategy_IsCorrectFor4D_FreshSoftHands()
		{
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 13, [["2", "3", "4", "7", "8", "9", "10", "J", "Q", "K", "A"], ["5", "6"]], [ActionEnum.H, ActionEnum.D], issoft: true));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 14, [["2", "3", "4", "7", "8", "9", "10", "J", "Q", "K", "A"], ["5", "6"]], [ActionEnum.H, ActionEnum.D], issoft: true));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 15, [["2", "3", "7", "8", "9", "10", "J", "Q", "K", "A"], ["4", "5", "6"]], [ActionEnum.H, ActionEnum.D], issoft: true));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 16, [["2", "3", "7", "8", "9", "10", "J", "Q", "K", "A"], ["4", "5", "6"]], [ActionEnum.H, ActionEnum.D], issoft: true));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 17, [["2", "7", "8", "9", "10", "J", "Q", "K", "A"], ["3", "4", "5", "6"]], [ActionEnum.H, ActionEnum.D], issoft: true));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 18, [["2", "3", "4", "5", "6"], ["7", "8"], ["9", "10", "J", "Q", "K", "A"]], [ActionEnum.D, ActionEnum.S, ActionEnum.H], issoft: true));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 19, [["2", "3", "4", "5", "7", "8", "9", "10", "J", "Q", "K", "A"], ["6"]], [ActionEnum.S, ActionEnum.D], issoft: true));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 20, [Card.RANKS], [ActionEnum.S], issoft: true));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 21, [Card.RANKS], [ActionEnum.S], issoft: true));
		}

		[TestMethod]
		public void BasicStrategy_IsCorrectFor4D_HitSoftHands()
		{
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 13, [Card.RANKS], [ActionEnum.H], issoft: true, isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 14, [Card.RANKS], [ActionEnum.H], issoft: true, isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 15, [Card.RANKS], [ActionEnum.H], issoft: true, isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 16, [Card.RANKS], [ActionEnum.H], issoft: true, isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 17, [Card.RANKS], [ActionEnum.H], issoft: true, isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 18, [["2", "3", "4", "5", "6", "7", "8"], ["9", "10", "J", "Q", "K", "A"]], [ActionEnum.S, ActionEnum.H], issoft: true, isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 19, [Card.RANKS], [ActionEnum.S], issoft: true, isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 20, [Card.RANKS], [ActionEnum.S], issoft: true, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 21, [Card.RANKS], [ActionEnum.S], issoft: true, hits: 1));
		}

		[TestMethod]
		public void BasicStrategy_IsCorrectFor4D_HitHardHands()
		{
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 4, [Card.RANKS], [ActionEnum.H], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 5, [Card.RANKS], [ActionEnum.H], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 6, [Card.RANKS], [ActionEnum.H], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 7, [Card.RANKS], [ActionEnum.H], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 8, [Card.RANKS], [ActionEnum.H], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 9, [Card.RANKS], [ActionEnum.H], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 10, [Card.RANKS], [ActionEnum.H], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 11, [Card.RANKS], [ActionEnum.H], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 12, [["2", "3", "7", "8", "9", "10", "J", "Q", "K", "A"], ["4", "5", "6"]], [ActionEnum.H, ActionEnum.S], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 13, [["2", "3", "4", "5", "6"], ["7", "8", "9", "10", "J", "Q", "K", "A"]], [ActionEnum.S, ActionEnum.H], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 14, [["2", "3", "4", "5", "6"], ["7", "8", "9", "10", "J", "Q", "K", "A"]], [ActionEnum.S, ActionEnum.H], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 15, [["2", "3", "4", "5", "6"], ["7", "8", "9", "10", "J", "Q", "K", "A"]], [ActionEnum.S, ActionEnum.H], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 16, [["2", "3", "4", "5", "6"], ["7", "8", "9", "10", "J", "Q", "K", "A"]], [ActionEnum.S, ActionEnum.H], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 17, [Card.RANKS], [ActionEnum.S], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 18, [Card.RANKS], [ActionEnum.S], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 19, [Card.RANKS], [ActionEnum.S], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 20, [Card.RANKS], [ActionEnum.S], isnew: false, hits: 1));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, 21, [Card.RANKS], [ActionEnum.S], isnew: false, hits: 1));
		}

		[TestMethod]
		public void BasicStrategy_IsCorrectFor_4DSplits()
		{
			var rules = new BlackjackRules(6, 70)
			{
				IsSurrenderAllowed = true,
				CanResplitAces = true,
			};

			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, new TestHand(4) { Cards = [new("2"), new("2")], CanSplit = true, IsNewHand = true }, [["2", "3", "4", "5", "6", "7"], ["8", "9", "10", "J", "Q", "K", "A"]], [ActionEnum.P, ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, new TestHand(6) { Cards = [new("3"), new("3")], CanSplit = true, IsNewHand = true }, [["2", "3", "4", "5", "6", "7"], ["8", "9", "10", "J", "Q", "K", "A"]], [ActionEnum.P, ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, new TestHand(8) { Cards = [new("4"), new("4")], CanSplit = true, IsNewHand = true }, [["2", "3", "4"], ["5", "6"], ["7", "8", "9", "10", "J", "Q", "K", "A"]], [ActionEnum.H, ActionEnum.P, ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, new TestHand(10) { Cards = [new("5"), new("5")], CanSplit = true, IsNewHand = true }, [["2", "3", "4", "5", "6", "7", "8", "9"], ["10", "J", "Q", "K", "A"]], [ActionEnum.D, ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, new TestHand(12) { Cards = [new("6"), new("6")], CanSplit = true, IsNewHand = true }, [["2", "3", "4", "5", "6"], ["7", "8", "9", "10", "J", "Q", "K", "A"]], [ActionEnum.P, ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, new TestHand(14) { Cards = [new("7"), new("7")], CanSplit = true, IsNewHand = true }, [["2", "3", "4", "5", "6", "7"], ["8", "9", "10", "J", "Q", "K", "A"]], [ActionEnum.P, ActionEnum.H]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, new TestHand(16) { Cards = [new("8"), new("8")], CanSplit = true, IsNewHand = true }, [["2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"], ["A"]], [ActionEnum.P, ActionEnum.R]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, new TestHand(18) { Cards = [new("9"), new("9")], CanSplit = true, IsNewHand = true }, [["2", "3", "4", "5", "6", "8", "9"], ["7", "10", "J", "Q", "K", "A"]], [ActionEnum.P, ActionEnum.S]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, new TestHand(20) { Cards = [new("10"), new("10")], CanSplit = true, IsNewHand = true }, [Card.RANKS], [ActionEnum.S]));
			Assert.IsTrue(TestRowResult(BASICSTRATEGY_4D, new GameState { Rules = rules, PlayerHand = new TestHand(12) { Cards = [new("A"), new("A")], CanSplit = true, IsNewHand = true } }, [Card.RANKS], [ActionEnum.P]));
		}

		[TestMethod]
		public void TakeInsurance_IsCorrectForBasicStrategy()
		{
			GameState gamestate = new GameState { TrueCount = 0 };
			Assert.IsFalse(BASICSTRATEGY_2D.DoTakeInsurrance(gamestate));
			gamestate.TrueCount = 3;
			Assert.IsFalse(BASICSTRATEGY_2D.DoTakeInsurrance(gamestate));
		}

		[TestMethod]
		public void TakeInsurance_IsCorrectForDeviatedStrategy()
		{
			GameState gamestate = new GameState { TrueCount = 0 };
			Assert.IsFalse(BASICSTRATEGY_2D_WITHDEVIATIONS.DoTakeInsurrance(gamestate));
			gamestate.TrueCount = 3;
			Assert.IsTrue(BASICSTRATEGY_2D_WITHDEVIATIONS.DoTakeInsurrance(gamestate));
		}

		[TestMethod]
		public void DealerStrategy_IsCorrectForHardHands_H17()
		{
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_H17, 4, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_H17, 5, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_H17, 6, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_H17, 7, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_H17, 8, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_H17, 9, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_H17, 10, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_H17, 11, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_H17, 12, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_H17, 13, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_H17, 14, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_H17, 15, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_H17, 16, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_H17, 17, [Card.RANKS], [ActionEnum.S]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_H17, 18, [Card.RANKS], [ActionEnum.S]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_H17, 19, [Card.RANKS], [ActionEnum.S]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_H17, 20, [Card.RANKS], [ActionEnum.S]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_H17, 21, [Card.RANKS], [ActionEnum.S]));
		}

		[TestMethod]
		public void DealerStrategy_IsCorrectForSoftHands_H17()
		{
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_H17, 13, [Card.RANKS], [ActionEnum.H], issoft: true));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_H17, 14, [Card.RANKS], [ActionEnum.H], issoft: true));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_H17, 15, [Card.RANKS], [ActionEnum.H], issoft: true));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_H17, 16, [Card.RANKS], [ActionEnum.H], issoft: true));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_H17, 17, [Card.RANKS], [ActionEnum.H], issoft: true));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_H17, 18, [Card.RANKS], [ActionEnum.S], issoft: true));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_H17, 19, [Card.RANKS], [ActionEnum.S], issoft: true));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_H17, 20, [Card.RANKS], [ActionEnum.S], issoft: true));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_H17, 21, [Card.RANKS], [ActionEnum.S], issoft: true));
		}

		[TestMethod]
		public void DealerStrategy_IsCorrectForHardHands_S17()
		{
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_S17, 4, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_S17, 5, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_S17, 6, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_S17, 7, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_S17, 8, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_S17, 9, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_S17, 10, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_S17, 11, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_S17, 12, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_S17, 13, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_S17, 14, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_S17, 15, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_S17, 16, [Card.RANKS], [ActionEnum.H]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_S17, 17, [Card.RANKS], [ActionEnum.S]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_S17, 18, [Card.RANKS], [ActionEnum.S]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_S17, 19, [Card.RANKS], [ActionEnum.S]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_S17, 20, [Card.RANKS], [ActionEnum.S]));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_S17, 21, [Card.RANKS], [ActionEnum.S]));
		}

		[TestMethod]
		public void DealerStrategy_IsCorrectForSoftHands_S17()
		{
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_S17, 13, [Card.RANKS], [ActionEnum.H], issoft: true));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_S17, 14, [Card.RANKS], [ActionEnum.H], issoft: true));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_S17, 15, [Card.RANKS], [ActionEnum.H], issoft: true));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_S17, 16, [Card.RANKS], [ActionEnum.H], issoft: true));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_S17, 17, [Card.RANKS], [ActionEnum.S], issoft: true));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_S17, 18, [Card.RANKS], [ActionEnum.S], issoft: true));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_S17, 19, [Card.RANKS], [ActionEnum.S], issoft: true));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_S17, 20, [Card.RANKS], [ActionEnum.S], issoft: true));
			Assert.IsTrue(TestRowResult(DEALERSTRATEGY_S17, 21, [Card.RANKS], [ActionEnum.S], issoft: true));
		}
	}
}

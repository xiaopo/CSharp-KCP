using UnityEngine;
using System.Collections;

namespace BT {
	/*
		Sequence同样是一个逻辑节点，它的意思是从左到右按顺序执行子节点，
		并且仅仅在一个子节点执行完成后才执行下一个子节点。

		在例子中，Do Run需要有一个自己判断到达目的地的方法，当该方法返回end的时候，才会执行Do Slash。
	*/
	/// <summary>
	/// BTSequence evaluteas the current active child, or the first child (if no active child).
	/// 
	/// If passed the evaluation, BTSequence ticks the current active child, or the first child (if no active child available),
	/// and if it's result is BTEnded, then change the active child to the next one.
	/// 
	/// </summary>
	public class BTSequence : BTNode {
	
		private BTNode _activeChild;
		private int _activeIndex = -1;
		
		
		public BTSequence (BTPrecondition precondition = null) : base (precondition) {}
		
		protected override bool DoEvaluate () {
			if (_activeChild != null) {
				bool result = _activeChild.Evaluate();
				if (!result) {
					_activeChild.Clear();
					_activeChild = null;
					_activeIndex = -1;
				}
				return result;
			}
			else {
				return children[0].Evaluate();
			}
		}
		
		public override BTResult Tick () {
			// first time
			if (_activeChild == null) {
				_activeChild = children[0];
				_activeIndex = 0;
			}

			BTResult result = _activeChild.Tick();
			if (result == BTResult.Ended) {	// Current active node over
				_activeIndex++;
				if (_activeIndex >= children.Count) {	// sequence is over
					_activeChild.Clear();
					_activeChild = null;
					_activeIndex = -1;
				}
				else {	// next node
					_activeChild.Clear();
					_activeChild = children[_activeIndex];
					result = BTResult.Running;
				}
			}
			return result;
		}
		
		public override void Clear () {
			if (_activeChild != null) {
				_activeChild = null;
				_activeIndex = -1;
			}

			foreach (BTNode child in children) {
				child.Clear();
			}
		}
	}

}
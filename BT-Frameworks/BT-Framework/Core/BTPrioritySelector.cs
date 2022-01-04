using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BT {


	 /*
		 Priority Selector是一个逻辑节点它的意思是让从左到右遍历自己的子节点，
		 如果子节点的准入条件符合信息的话，就执行该子节点。
		 
	 */

	public class BTPrioritySelector : BTNode {
		
		private BTNode _activeChild;

		public BTPrioritySelector (BTPrecondition precondition = null) : base (precondition) {}

		// selects the active child
		protected override bool DoEvaluate () 
		{

			foreach (BTNode child in children) 
			{
				if (child.Evaluate()) 
				{
					if (_activeChild != null && _activeChild != child) {
						_activeChild.Clear();	
					}
					_activeChild = child;
					return true;
				}
			}

			if (_activeChild != null) {
				_activeChild.Clear();
				_activeChild = null;
			}

			return false;
		}
		
		public override void Clear () {
			if (_activeChild != null) {
				_activeChild.Clear();
				_activeChild = null;
			}

		}
		
		public override BTResult Tick () {
			if (_activeChild == null) {
				return BTResult.Ended;
			}

			BTResult result = _activeChild.Tick();
			if (result != BTResult.Running) {
				_activeChild.Clear();
				_activeChild = null;
			}
			return result;
		}
	}
}
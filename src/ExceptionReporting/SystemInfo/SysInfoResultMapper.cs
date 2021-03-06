using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ExceptionReporting.SystemInfo
{
	internal interface ISysInfoResultMapper
	{
		string SysInfoString();
	}

	///<summary>
	/// Map SysInfoResults to human-readable format
	///</summary>
	internal class SysInfoResultMapper : ISysInfoResultMapper
	{
		private readonly IEnumerable<SysInfoResult> _sysInfoResults;

		public SysInfoResultMapper(IEnumerable<SysInfoResult> sysInfoResults)
		{
			_sysInfoResults = sysInfoResults;
		}

		/// <summary>
		/// create a string representation of a list of SysInfoResults, customised specifically (eg 2-level deep)
		/// </summary>
		public string SysInfoString()
		{
			var sb = new StringBuilder();

			foreach (var result in _sysInfoResults)
			{
				sb.AppendLine(result.Name);

				foreach (var nodeValueParent in result.Nodes)
				{
					sb.AppendLine("-" + nodeValueParent);

					foreach (var childResult in result.ChildResults)
					{
						foreach (var nodeValue in childResult.Nodes)
						{
							sb.AppendLine("--" + nodeValue);		// the max no. of levels is 2, ie '--' is as deep as we go
						}
					}
				}
				sb.AppendLine();
			}

			return sb.ToString();
		}

		/// <summary>
		/// Add a tree node to an existing parentNode, by passing the SysInfoResult
		/// </summary>
		public static void AddTreeViewNode(TreeNode parentNode, SysInfoResult result)
		{
			var nodeRoot = new TreeNode(result.Name);

			foreach (var nodeValueParent in result.Nodes)
			{
				var nodeLeaf = new TreeNode(nodeValueParent);
				nodeRoot.Nodes.Add(nodeLeaf);

				foreach (var childResult in result.ChildResults)
				{
					foreach (var nodeValue in childResult.Nodes)
					{
						nodeLeaf.Nodes.Add(new TreeNode(nodeValue));
					}
				}
			}
			parentNode.Nodes.Add(nodeRoot);
		}
	}
}
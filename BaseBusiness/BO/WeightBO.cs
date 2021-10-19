
using System;
using System.Collections;
using BMS.Facade;
using BMS.Model;
namespace BMS.Business
{

	
	public class WeightBO : BaseBO
	{
		private WeightFacade facade = WeightFacade.Instance;
		protected static WeightBO instance = new WeightBO();

		protected WeightBO()
		{
			this.baseFacade = facade;
		}

		public static WeightBO Instance
		{
			get { return instance; }
		}
		
	
	}
}
	
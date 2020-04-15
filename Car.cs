using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoDataScraper
{
    enum Fuel
    {
        unknown,
        gas,
        electric,
        diesel
    }

    enum Gear
    {
        unknown,
        automatic,
        manual
    }

    enum BodyStyle
    {
        unknown,
        sedan,
        hatchback,
        SUV,
        coupe,
        convertible,
        wagon,
        van,
        jeep,
        transporter
    }

    enum Transmition
    {
        unknown,
        fwd,
        rwd,
        awd
    }

    enum Condition
    {
        unknown,
        fine,
        defected
    }

    class Car
    {
        public string m_Make;
        public string m_Model;
        public int m_Year;
        public Fuel m_Fuel;
        public Gear m_Gear;
        public BodyStyle m_BodyStyle;
        public Transmition m_Transmition;
        public Condition m_Condition;
        public int m_kW;
        public int m_Price;
        public int m_Mileage;
        public List<string> m_PhotoUrls;

        public Car()
        {
            m_Make = string.Empty;
            m_Model = string.Empty;
            m_Year = int.MinValue;
            m_Fuel = Fuel.unknown;
            m_Gear = Gear.unknown;
            m_BodyStyle = BodyStyle.unknown;
            m_Transmition = Transmition.unknown;
            m_Condition = Condition.fine;
            m_kW = int.MinValue;
            m_Price = int.MinValue;
            m_Mileage = int.MinValue;
            m_PhotoUrls = new List<string>();
        }
    }
}

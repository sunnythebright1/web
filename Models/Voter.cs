using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using ElectoralSystem.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ElectoralSystem.Models
{
    public class Voter
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public decimal NIC { get; set; }
        public bool OTPB { get; set; }
        public string Name { get; set; }
        public int areaCode { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public int naNo { get; set; }
        public string assemblyAlias { get; set; }
        public int paNo { get; set; }

        //Voter Details end

            //Candidate Detail starts

            public int cNo { get; set; }
        public string cName { get; set; }
        public string partyName { get; set; }
         public string symbolPath { get; set; }
        public string cAssemblyAlias { get; set; }
        public int cProvincialNo { get; set; }


        public int nNo { get; set; }
        public string nName { get; set; }
        public string npartyName { get; set; }
        public string nsymbolPath { get; set; }
        public int NA { get; set; }
        List<Voter> voters = new List<Voter>();

        public void getVoter()
        {
            SqlCommand cmd = new SqlCommand("stp_votingDetails", Connection.connect());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NIC", NIC);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {   

                Name = (string)reader[0];
              areaCode = Convert.ToInt32(reader[1]);
                Area = (string)reader[2];
                City = (string)reader[3];
                naNo = Convert.ToInt32(reader[4]);
                assemblyAlias = (string)(reader[5]);
                paNo = Convert.ToInt32(reader[6]);

            }
        }
        public bool authenticate(decimal _NIC, int _OTP)
        {
            SqlCommand cmd = new SqlCommand("Select OTP from General_Public where NIC_NO =" + _NIC + "AND OTP =" + _OTP, Connection.connect());
            SqlDataReader reader = cmd.ExecuteReader();

            /*     
                 if (reader.Read())
                 {
                     OTPB = true;
                 }
                 else
                 {
                     OTPB = false;
                 }
                 */
            if (reader.Read())
            {
                return true;

            }
            else return false;
        }
  /*      public void voteConstituency()
        {
            SqlCommand cmd = new SqlCommand("stp_voteConstituency", Connection.connect());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Area_Code",areaCode);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cName = (string)reader["Name"];
                partyName = (string)reader["Party_Name"];
                symbolPath = (string)reader["Symbol_Path"];
                cAssemblyAlias = (string)reader["Assembly_Alias"];
                cProvincialNo = Convert.ToInt16(reader["Provincial_No"]);
                
            }
            
        }
        */
        public List<Voter> voteConstituency()
        {
            Voter v = null;
            SqlCommand cmd = new SqlCommand("stp_voteConstituency", Connection.connect());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Area_Code", areaCode);
            SqlDataReader reader = cmd.ExecuteReader();
            
            
            while (reader.Read())
            {
                v = new Voter();
                v.cNo=Convert.ToInt32(reader["Candidate_No"]);
               v.cName = (string)reader["Name"];
                v.partyName = (string)reader["Party_Name"];
                v.symbolPath = (string)reader["Symbol_Path"];
                v.cAssemblyAlias = (string)reader["Assembly_Alias"];
                v.cProvincialNo = Convert.ToInt16(reader["Provincial_No"]);
                voters.Add(v);

            }
            return voters;
        }
        public List<Voter> voteNA()
        {

            //getting NA no thru area code
            SqlCommand cmd1 = new SqlCommand("Select National_Assembly from Area_Master where Area_Code=" + areaCode, Connection.connect());
            SqlDataReader reader1 = cmd1.ExecuteReader();
            if(reader1.Read())
            {
                NA = Convert.ToInt32(reader1[0]);
            }
            Voter v = null;
            SqlCommand cmd = new SqlCommand("stp_voteNA", Connection.connect());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NA", NA);
            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                v = new Voter();
                v.nNo = Convert.ToInt32(reader["Candidate_No"]);
                v.nName = (string)reader["Name"];
                v.npartyName = (string)reader["Party_Name"];
                v.nsymbolPath = (string)reader["Symbol_Path"];
                v.NA = Convert.ToInt16(reader["National_Assembly"]);
           
                voters.Add(v);

            }
            return voters;
        }

    }

}
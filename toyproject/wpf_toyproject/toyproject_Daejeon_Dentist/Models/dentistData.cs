using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace toyproject_Daejeon_Dentist.Models
{
     class dentistData
    {
        /*
         * {"name":"대전광역시 서구_치과병의원 현황","description":"대전광역시 서구 관내에서 운영중인 치과병의원 현황 정보
         * (의료기관명, 종별, 주소, 행정동, 법정동, 위도, 경도 등)를 제공합니다.",
         * "url":"https://www.data.go.kr/data/15127960/openapi.do","keywords":["치과,병원,현황"],
         * "license":"https://data.go.kr/ugs/selectPortalPolicyView.do","dateCreated":"2024-05-07",
         * "dateModified":"2024-05-07","datePublished":"2024-05-07","creator":{"name":"대전광역시 서구",
         * "contactPoint":{"contactType":"홍보실","telephone":"+82-0422882392","@type":"ContactPoint"},
         * "@type":"Organization"},"distribution":[{"encodingFormat":"JSON+XML",
         * "contentUrl":"https://www.data.go.kr/data/15127960/openapi.do","@type":"DataDownload"}],"@context":"https://schema.org","@type":"Dataset"}
         * 
         * 순번	sn	numeric	순번	2
            의료기관명	mdlc_instt_nm	varchar(50)	의료기관명	서울방치과의원
            의료기관종별명	mdlc_instt_asort_nm	varchar(30)	의료기관종별명	치과의원
            지번주소	lnm_adrs	varchar(100)	지번주소	대전광역시 서구 괴정동 423-4
            도로명주소	rn_adrs	varchar(100)	도로명주소	대전광역시 서구 계룡로 616
            행정동코드	admd_cd	varchar(10)		
            행정동명	admd_nm	varchar(10)		
            법정동코드	lgdng_cd	varchar(10)		
            법정동명	lgdng_nm	varchar(10)		
            전화번호	telno	varchar(30)	전화번호	042-528-7825
            위도	la	numeric		
            경도	lo	numeric		
            데이터기준일자	data_stdr_de	varchar(10)		

         */


        public static readonly string CHECK_QUERY = @"SELECT COUNT(*) 
                                                      FROM dentist
                                                     WHERE Sn = @Sn";
  
        public int Sn { get; set; }     // 순번
        public string Mdlc_instt_nm { get; set; }   // 의료기관명
        public string Mdlc_instt_asort_nm { get; set; }     // 의료기관종별명
        public string Rn_adrs { get; set; }     // 도로명 주소
        public string Telno { get; set; }       // 전화번호
        public string Lnm_adrs { get; set; }    // 지번 주소

        public static readonly string INSERT_QUERY = @"INSERT INTO [dbo].[dentist]
                                                                   ([Sn]
                                                                   ,[Mdlc_instt_nm]
                                                                   ,[Mdlc_instt_asort_nm]
                                                                   ,[Rn_adrs]
                                                                   ,[Telno]
                                                                   ,[Lnm_adrs])
                                                             VALUES
                                                                   (@Sn
                                                                   ,@Mdlc_instt_nm
                                                                   ,@Mdlc_instt_asort_nm
                                                                   ,@Rn_adrs
                                                                   ,@Telno
                                                                   ,@Lnm_adrs)";

        public static readonly string SELECT_QUERY = @"SELECT  [Sn]
                                                              ,[Mdlc_instt_nm]
                                                              ,[Mdlc_instt_asort_nm]
                                                              ,[Rn_adrs]
                                                              ,[Telno]
                                                              ,[Lnm_adrs]
                                                          FROM [dbo].[dentist]";
                                                    

        public static readonly string GETNAME_QUERY = @"SELECT Mdlc_instt_nm
                                                          FROM [dbo].[dentist]";

        public static readonly string DELETE_QUERY = @"DELETE FROM [dbo].[dentist] WHERE Sn= @Sn";



    }
}

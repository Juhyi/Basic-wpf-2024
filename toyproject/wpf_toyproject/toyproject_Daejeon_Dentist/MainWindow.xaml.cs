using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using toyproject_Daejeon_Dentist.Models;

namespace toyproject_Daejeon_Dentist
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private bool isFavorite = false;

        

        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitComboDateFromDB()
        {
            using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(Models.dentistData.GETNAME_QUERY, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet dSet = new DataSet();
                adapter.Fill(dSet);
                List<string> saveDates = new List<string>();

                foreach (DataRow row in dSet.Tables[0].Rows)
                {
                    saveDates.Add(Convert.ToString(row["Save_Date"]));
                }

                CboRepData.ItemsSource = saveDates;
            }
        }

        // 조회 버큰 클릭
        private async void BtnSerch_Click(object sender, RoutedEventArgs e)
        {
            string openApiUri = "\thttps://www.seogu.go.kr/seoguAPI/3660000/getDsgrAsmby";
            string result = string.Empty;

            WebRequest req = null;
            WebResponse res = null;
            StreamReader reader = null;

            try
            {
                req = WebRequest.Create(openApiUri);
                res = await req.GetResponseAsync();
                reader = new StreamReader(res.GetResponseStream());
                result = reader.ReadToEnd();

               
                Debug.WriteLine(result);
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("오류", $"OpenApi 조회오류 {ex.Message} ");
            }

            var jsonResult = JObject.Parse(result);
            var status = Convert.ToString( jsonResult["response"]["header"]["resultCode"]);

            if (status == "C00")
            {
                var data = jsonResult["response"]["body"]["items"];
                var jsonArray = data as JArray; // json자체에서 []안에 들어간 배열데이터만 JArray 변환가능

                var dentistData = new List<dentistData>();
                foreach (var item in jsonArray)
                {
                    dentistData.Add(new dentistData()
                    {
                        Rn_adrs = Convert.ToString(item["rn_adrs"]),
                        Mdlc_instt_nm = Convert.ToString(item["mdlc_instt_nm"]),
                        Mdlc_instt_asort_nm = Convert.ToString(item["mdlc_instt_asort_nm"]),
                        Sn = Convert.ToInt32(item["sn"]),
                        Lnm_adrs = Convert.ToString(item["lnm_adrs"]),
                        Telno = Convert.ToString(item["telno"]),

                    });
                }
                this.DataContext = dentistData;
                StsResult.Content = $"OpenAPI {dentistData.Count}건 조회 완료.";
            }
        }
        // 나의 병원 추가 버튼 
        private async void BtnAddMydentist_Click(object sender, RoutedEventArgs e)
        {
            if (GrdResult.SelectedItems.Count == 0)
            {
                await this.ShowMessageAsync("나의 병원", "추가할 병원을 선택하세요(복수 선택 가능)");
                return;
            }

            if (isFavorite == true)  // 즐겨찾기 보기한 뒤 영화를 다시 즐겨찾기하려고할때 막음
            {
                await this.ShowMessageAsync("나의 병원", "이미 저장된 병원입니다.");
                return;
            }

            var adddentists = new List<dentistData>();
            foreach (dentistData item in GrdResult.SelectedItems)
            {
                adddentists.Add(item);
            }

        
            try
            {
                var insRes = 0;

                using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
                {
                    conn.Open();

                    foreach (dentistData item in adddentists)
                    {
                       
                        SqlCommand chkCmd = new SqlCommand(dentistData.CHECK_QUERY, conn);
                        chkCmd.Parameters.AddWithValue("@Sn", item.Sn);
                        var cnt = Convert.ToInt32(chkCmd.ExecuteScalar());   

                        if (cnt == 1) continue; 

                        SqlCommand cmd = new SqlCommand(Models.dentistData.INSERT_QUERY, conn);
                        cmd.Parameters.AddWithValue("@Sn", item.Sn);
                        cmd.Parameters.AddWithValue("@Mdlc_instt_nm", item.Mdlc_instt_nm);
                        cmd.Parameters.AddWithValue("@Mdlc_instt_asort_nm", item.Mdlc_instt_asort_nm);
                        cmd.Parameters.AddWithValue("@Rn_adrs", item.Rn_adrs);
                        cmd.Parameters.AddWithValue("@Telno", item.Telno);
                        cmd.Parameters.AddWithValue("@Lnm_adrs", item.Lnm_adrs);
                       

                        insRes += cmd.ExecuteNonQuery();  

                    }
                }

                if (insRes == adddentists.Count)
                {
                    await this.ShowMessageAsync("즐겨찾기", $"즐겨찾기 {insRes}건 저장성공");
                }
                else
                {
                    await this.ShowMessageAsync("즐겨찾기", $"즐겨찾기 {adddentists.Count}건중 {insRes}건 저장성공");
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("오류", $"즐겨찾기 오류 { ex.Message }");
            }

            BtnViewMydentist_Click(sender, e);  
        }

       // 나의병원 조회버튼 클릭
        private async void BtnViewMydentist_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = null;    


            List<dentistData> myDentist = new List<dentistData>();

            try
            {
                using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
                {
                    conn.Open();

                    var cmd = new SqlCommand(Models.dentistData.SELECT_QUERY, conn);
                    var adapter = new SqlDataAdapter(cmd);
                    var dSet = new DataSet();
                    adapter.Fill(dSet, "dentistData");


                    foreach (DataRow row in dSet.Tables["dentistData"].Rows)
                    {
                        var dentistData = new dentistData()
                        {
                            Rn_adrs = Convert.ToString(row["Rn_adrs"]),
                            Mdlc_instt_nm = Convert.ToString(row["Mdlc_instt_nm"]),
                            Mdlc_instt_asort_nm = Convert.ToString(row["Mdlc_instt_asort_nm"]),
                            Sn = Convert.ToInt32(row["Sn"]),
                            Lnm_adrs = Convert.ToString(row["Lnm_adrs"]),
                            Telno = Convert.ToString(row["Telno"])
                        };
                        myDentist.Add(dentistData);
                    }
                    this.DataContext = myDentist;
                    isFavorite = true;  //즐겨찾기 db에서
                    StsResult.Content = $"즐겨찾기 {myDentist.Count}건 조회 완료";
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("즐겨찾기", $"즐겨찾기 조회오류 {ex.Message}");
            }
        }

     // 삭제 버튼 클릭
        private async void BtnDelFavorite_Click(object sender, RoutedEventArgs e)
        {
            //await this.ShowMessageAsync("즐겨찾기", "즐겨찾기 목록에서 삭제");
            if (isFavorite == false)
            {
                await this.ShowMessageAsync("삭제", "즐겨찾기한 병원이 아닙니다.");
                return;
            }

            if (GrdResult.SelectedItems.Count == 0)
            {
                await this.ShowMessageAsync("삭제", "삭제할 병원을 선택하세요.");
                return;
            }

            // 삭제 시작
            try
            {
                using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
                {
                    conn.Open();

                    var delRes = 0;

                    foreach (dentistData item in GrdResult.SelectedItems)
                    {
                        SqlCommand cmd = new SqlCommand(Models.dentistData.DELETE_QUERY, conn);
                        cmd.Parameters.AddWithValue("@Sn", item.Sn);

                        delRes += cmd.ExecuteNonQuery();
                    }

                    if (delRes == GrdResult.SelectedItems.Count)
                    {
                        await this.ShowMessageAsync("삭제", $"즐겨찾기 {delRes}건 삭제");
                    }
                    else
                    {
                        await this.ShowMessageAsync("삭제", $"즐겨찾기 {GrdResult.SelectedItems.Count}건중 {delRes}건 삭제");
                    }
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("오류", $"즐겨찾기 삭제 오류 {ex.Message}");
            }

            BtnViewMydentist_Click(sender, e);   //즐겨찾기 보기 재실행
        }
        private void CboRepData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        // 그리드 선택 후 더블클릭
        private async void GrdResult_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine(GrdResult.SelectedItem);
            var curItem = GrdResult.SelectedItem as dentistData;

            await this.ShowMessageAsync($"{curItem.Mdlc_instt_nm} ({curItem.Telno})", $"도로명 주소: {curItem.Rn_adrs}\n 지번 주소: {curItem.Lnm_adrs}");
        }

       

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
        }



        private void GrdResult_SelectedCellsChanged(object Sender, SelectedCellsChangedEventArgs e) 
        {
            var curItem = GrdResult.SelectedItem as dentistData;
            BrsLoc.Address = $"http://google.com/maps/place/{curItem.Rn_adrs}";
        }
    }
}
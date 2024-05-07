using Caliburn.Micro;
using ex07_EmployeeMngApp.Helpers;
using ex07_EmployeeMngApp.Models;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Data.SqlClient;
using System.Windows;

namespace ex07_EmployeeMngApp.ViewModels
{
    public class MainViewModel : Conductor<object>
    {
        private IDialogCoordinator _dialogCoordinator;  // 메트로방식 다이얼로그 디자인 적용 객체

        // 멤버변수
        private int id;
        private string empName;
        private decimal salary;
        private string deptName;
        private string addr;

        // List는 정적인 컬렉션, BindableCollection은 동적인 컬렉션.
        // MVVM처럼 List를 사용못함
        private BindableCollection<Employees> listEmployees;

        private Employees selectedEmployee;

        // 속성 
        public int Id
        {
            get => id; 
            set
            {
                id = value;
                NotifyOfPropertyChange(() => id);
                NotifyOfPropertyChange(() => CanDelEmployee); // 삭제 여부 속성도 변경했다 공지

            }
        }
        public string EmpName
        {
            get => empName; set
            {
                empName = value;
                NotifyOfPropertyChange(() => EmpName);
                NotifyOfPropertyChange(() => CanSaveEmployee);
            }
        }
        public decimal Salary
        {
            get => salary; set
            {
                salary = value;
                NotifyOfPropertyChange(() => Salary);
                NotifyOfPropertyChange(() => CanSaveEmployee);


            }
        }
        public string DeptName
        {
            get => deptName; set
            {
                deptName = value;
                NotifyOfPropertyChange(() => DeptName);
                NotifyOfPropertyChange(() => CanSaveEmployee);

            }
        }
        public string Addr
        {
            get => addr; set
            {
                addr = value;
                NotifyOfPropertyChange(() => Addr);

            }
        }

        // DataGrid에 뿌릴 Employees 테이블 데이터
        public BindableCollection<Employees> ListEmployees
        { 
            get => listEmployees;
            set
            {
                listEmployees = value;
                // 값이 변경된 것을 시스템 알려줌
                NotifyOfPropertyChange(()=>ListEmployees); // 필수
            }
        }

        public Employees SelectedEmployee
        {
            get => selectedEmployee; 
            set
            {
                selectedEmployee = value;
                // 데이털를 TextBox들에 전달.

                if(selectedEmployee != null)    
                {
                    Id = value.Id;
                    EmpName = value.EmpName;
                    Salary = value.Salary;
                    DeptName = value.DeptName;
                    Addr = value.Addr;

                    NotifyOfPropertyChange(() => SelectedEmployee); // View에 데이터가 표시되려면 필수!
                    NotifyOfPropertyChange(()=> Id);
                    NotifyOfPropertyChange(()=> EmpName);
                    NotifyOfPropertyChange(()=> Salary);
                    NotifyOfPropertyChange(()=> DeptName);
                    NotifyOfPropertyChange(()=> Addr);
                }    
            }
        }

        public MainViewModel()
        {
            DisplayName = "직원관리 시스템";

            // 조회실행 
            GetEmployees();
        }

        //저장버튼 활성화 여부 속성
        public bool CanSaveEmployee
        {
            get
            {
                if (string.IsNullOrEmpty(EmpName) || Salary == 0 || string.IsNullOrEmpty(DeptName))
                    return false;
                else
                    return true;
            }
        }

        /// <summary>
        /// Cliburn.Micro가 Xaml의 버튼 x:Name과 동일한 메서드로 매핑
        /// </summary>

        public async void SaveEmployee()
        {
            if(Common.DialogCoordinator != null) 
            {
                this._dialogCoordinator = Common.DialogCoordinator;
            }

            //MessageBox.Show("저장버튼 동작");
            using(SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING)) 
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                if( Id == 0) //INSERT
                    cmd.CommandText = Models.Employees.INSERT_QUERY;
                else // UPDATE
                    cmd.CommandText = Models.Employees.UPDATE_QUERY;

                SqlParameter prmEmpName = new SqlParameter("@EmpName", EmpName);
                cmd.Parameters.Add(prmEmpName);
                SqlParameter prmSalary = new SqlParameter("@Salary", Salary);
                cmd.Parameters.Add(prmSalary); 
                SqlParameter prmDeptName = new SqlParameter("@DeptName", DeptName);
                cmd.Parameters.Add(prmDeptName);
                SqlParameter prmAddr = new SqlParameter("@Addr", Addr ?? (object)DBNull.Value);
                cmd.Parameters.Add(prmAddr);

                if(Id != 0) // 업데이트면 Id 파라미터가 필요
                {
                    SqlParameter prmId = new SqlParameter("@Id", Id);    
                    cmd.Parameters.Add(prmId);
                }

                var result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    //MessageBox.Show("저장성공!");
                    await this._dialogCoordinator.ShowMessageAsync(this, "저장성공!" ,"저장");
                }
                else
                {
                    //MessageBox.Show("저장실패!");
                    await this._dialogCoordinator.ShowMessageAsync(this, "저장실패!", "저장");
                }
                GetEmployees(); // 그리드 재조회
                NewEmployee();
            }
        }

        public void GetEmployees()
        {
            using(SqlConnection conn = new SqlConnection((Helpers.Common.CONNSTRING)))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(Models.Employees.SLELECT_QUERY, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                ListEmployees = new BindableCollection<Employees>();

                while (reader.Read())
                {
                    ListEmployees.Add(new Employees()
                    {
                        Id = (int)reader["Id"],
                        EmpName = reader["EmpName"].ToString(),
                        Salary = (decimal)reader["Salary"],
                        DeptName = reader["DeptName"].ToString(),
                        Addr = reader["Addr"].ToString()

                    }) ;
                }
            }    
        }

        // 삭제버튼을 누를 수 있는지 여부확인
        public bool CanDelEmployee
        {
            get { return Id != 0; } // TextBox Id 속성의 값이 0이면 false, 0이 아니면 true
        }

        public async void DelEmployee()
        {
            if (Common.DialogCoordinator != null)
            {
                this._dialogCoordinator = Common.DialogCoordinator;
            }

            if (Id == 0) 
            {
                //MessageBox.Show("삭제 불가");
                await this._dialogCoordinator.ShowMessageAsync(this, "삭제 불가", "삭제");
                return;
            }

            var val = await this._dialogCoordinator.ShowMessageAsync(this, "삭제하시겟습니까?", "삭제여부", MessageDialogStyle.Affirmative);
            if( val == MessageDialogResult.Negative)
            {
                return;
            }

            // if(MessageBox.Show("삭제하시겠습니까?", "삭제여부", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            // {
            //    return;
            // }

            using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(Models.Employees.DELETE_QUERY, conn);
                SqlParameter prmId = new SqlParameter("@Id", Id);
                cmd.Parameters.Add(prmId);

                var res = cmd.ExecuteNonQuery();
                if(res >= 0)
                {
                    //MessageBox.Show("삭제 성공");
                    await this._dialogCoordinator.ShowMessageAsync(this, "삭제 성공", "삭제");
                }
                else
                {
                    //MessageBox.Show("삭제 실패");
                    await this._dialogCoordinator.ShowMessageAsync(this, "삭제 실패", "삭제");

                }

                GetEmployees();
                NewEmployee(); // 모든 입력 컨트롤 초기화
            }
        }

        public void NewEmployee()
        {
            Id = 0;
            Salary = 0;
            EmpName = DeptName = Addr = "";
        }
    } // class
} // namespace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Huellero.Models;
using SecuGen.FDxSDKPro.Windows;
using NuGet.Packaging;
using NuGet.Protocol.Plugins;
using Microsoft.AspNetCore.Components.Web;

namespace Huellero.Controllers
{
    public class TblUsuariosController : Controller
    {
        private readonly PruebaContext _context;
        private SGFingerPrintManager m_FPM;
        //private bool m_LedOn = false;
        //private Int32 m_ImageWidth;
        //private Int32 m_ImageHeight;
        //private Byte[] m_RegMin1 = null;
        //private Byte[] m_RegMin2;
        //private Byte[] m_VrfMin;
        private SGFPMDeviceList[] m_DevList;
        List<SelectListItem> ListaDispositivos;
        public TblUsuariosController(PruebaContext context)
        {
            _context = context;

            m_FPM = new SGFingerPrintManager();
            Listar_Dispositivos();
            AbrirDispositivo(0);
        }
         private List<SelectListItem> Listar_Dispositivos()
        {
            Int32 iError;
            string enum_device;
            

            // Enumerate Device
            iError = m_FPM.EnumerateDevice();

            // Get enumeration info into SGFPMDeviceList
            m_DevList = new SGFPMDeviceList[m_FPM.NumberOfDevice];
           ListaDispositivos = new List<SelectListItem>();

            for (int i = 0; i < m_FPM.NumberOfDevice; i++)
            {
                m_DevList[i] = new SGFPMDeviceList();
                m_FPM.GetEnumDeviceInfo(i, m_DevList[i]);
                enum_device = m_DevList[i].DevName.ToString() + " : " + m_DevList[i].DevID;

                ListaDispositivos.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = enum_device
                });

               
            }

      
            return ListaDispositivos;
        }




        // GET: TblUsuarios
        public async Task<IActionResult> Index()
        {
              return View(await _context.TblUsuarios.ToListAsync());
        }

        // GET: TblUsuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblUsuarios == null)
            {
                return NotFound();
            }

            var tblUsuario = await _context.TblUsuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblUsuario == null)
            {
                return NotFound();
            }

            return View(tblUsuario);
        }

        // GET: TblUsuarios/Create
        public IActionResult Create()
        {

            Int32 iError;
            string enum_device;


            // Enumerate Device
            iError = m_FPM.EnumerateDevice();

            // Get enumeration info into SGFPMDeviceList
            m_DevList = new SGFPMDeviceList[m_FPM.NumberOfDevice];
            ListaDispositivos = new List<SelectListItem>();

            for (int i = 0; i < m_FPM.NumberOfDevice; i++)
            {
                m_DevList[i] = new SGFPMDeviceList();
                m_FPM.GetEnumDeviceInfo(i, m_DevList[i]);
                enum_device = m_DevList[i].DevName.ToString() + " : " + m_DevList[i].DevID;

                ListaDispositivos.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = enum_device
                });


            }
            ViewBag.Dispositivoss = ListaDispositivos;
            return View();
        }

        // POST: TblUsuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VNombres,VApellidos,VHuella")] TblUsuario tblUsuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblUsuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblUsuario);
        }

        // GET: TblUsuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblUsuarios == null)
            {
                return NotFound();
            }

            var tblUsuario = await _context.TblUsuarios.FindAsync(id);
            if (tblUsuario == null)
            {
                return NotFound();
            }
            return View(tblUsuario);
        }

        // POST: TblUsuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VNombres,VApellidos,VHuella")] TblUsuario tblUsuario)
        {
            if (id != tblUsuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblUsuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblUsuarioExists(tblUsuario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tblUsuario);
        }

        // GET: TblUsuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblUsuarios == null)
            {
                return NotFound();
            }

            var tblUsuario = await _context.TblUsuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblUsuario == null)
            {
                return NotFound();
            }

            return View(tblUsuario);
        }

        // POST: TblUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TblUsuarios == null)
            {
                return Problem("Entity set 'PruebaContext.TblUsuarios'  is null.");
            }
            var tblUsuario = await _context.TblUsuarios.FindAsync(id);
            if (tblUsuario != null)
            {
                _context.TblUsuarios.Remove(tblUsuario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        private bool TblUsuarioExists(int id)
        {
          return _context.TblUsuarios.Any(e => e.Id == id);
        }

        //public static Task<JsonResult> Prueba(int idSeleccionado)
        //{
        //    //return  Json(AbrirDispositivo(idSeleccionado));
        //}

        public int  AbrirDispositivo(int ValorSeleccionado)
        {
            //if (m_FPM.NumberOfDevice == 0)
            //    return 1;

            SGFPMDeviceName device_name;
            int device_id;
            int iError;

            //Int32 numberOfDevices = comboBoxDeviceName.Items.Count;
            //Int32 deviceSelected = comboBoxDeviceName.SelectedIndex;
            //Boolean autoSelection = (deviceSelected == (numberOfDevices - 1));  // Last index

            //if (autoSelection)
            //{
            //    // Order of search: Hamster IV(HFDU04) -> Plus(HFDU03) -> III (HFDU02)
            //    device_name = SGFPMDeviceName.DEV_AUTO;

            //    device_id = (Int32)(SGFPMPortAddr.USB_AUTO_DETECT);
            //}
         
                device_name = m_DevList[ValorSeleccionado].DevName;
                device_id = m_DevList[ValorSeleccionado].DevID;

                iError = OpenDevice2(device_name, device_id);
            return iError;
        }


        private int OpenDevice2(SGFPMDeviceName device_name, int device_id)
        {
            int iError = m_FPM.Init(device_name);
            iError = m_FPM.OpenDevice(device_id);

           // CheckBoxAutoOn.Enabled = false;
            if (iError == (int)SGFPMError.ERROR_NONE)
            {
                //GetBtn_Click(sender, e);
                //GetBtn_Click(null, null);
                ViewBag.Estado = "Initialization Success";
                //EnableButtons(true);

                // FDU03, FDU04 or higher
                //if (device_name >= SGFPMDeviceName.DEV_FDU03)
                //    CheckBoxAutoOn.Enabled = true;
            }
            else
                DisplayError("OpenDevice()", iError);
            return iError;
        }


        void DisplayError(string funcName, int iError)
        {
            string text = "";

            switch (iError)
            {
                case 0:                             //SGFDX_ERROR_NONE				= 0,
                    text = "Error none";
                    break;

                case 1:                             //SGFDX_ERROR_CREATION_FAILED	= 1,
                    text = "Can not create object";
                    break;

                case 2:                             //   SGFDX_ERROR_FUNCTION_FAILED	= 2,
                    text = "Function Failed";
                    break;

                case 3:                             //   SGFDX_ERROR_INVALID_PARAM	= 3,
                    text = "Invalid Parameter";
                    break;

                case 4:                          //   SGFDX_ERROR_NOT_USED			= 4,
                    text = "Not used function";
                    break;

                case 5:                                //SGFDX_ERROR_DLLLOAD_FAILED	= 5,
                    text = "Can not create object";
                    break;

                case 6:                                //SGFDX_ERROR_DLLLOAD_FAILED_DRV	= 6,
                    text = "Can not load device driver";
                    break;
                case 7:                                //SGFDX_ERROR_DLLLOAD_FAILED_ALGO = 7,
                    text = "Can not load sgfpamx.dll";
                    break;

                case 51:                //SGFDX_ERROR_SYSLOAD_FAILED	   = 51,	// system file load fail
                    text = "Can not load driver kernel file";
                    break;

                case 52:                //SGFDX_ERROR_INITIALIZE_FAILED  = 52,   // chip initialize fail
                    text = "Failed to initialize the device";
                    break;

                case 53:                //SGFDX_ERROR_LINE_DROPPED		   = 53,   // image data drop
                    text = "Data transmission is not good";
                    break;

                case 54:                //SGFDX_ERROR_TIME_OUT			   = 54,   // getliveimage timeout error
                    text = "Time out";
                    break;

                case 55:                //SGFDX_ERROR_DEVICE_NOT_FOUND	= 55,   // device not found
                    text = "Device not found";
                    break;

                case 56:                //SGFDX_ERROR_DRVLOAD_FAILED	   = 56,   // dll file load fail
                    text = "Can not load driver file";
                    break;

                case 57:                //SGFDX_ERROR_WRONG_IMAGE		   = 57,   // wrong image
                    text = "Wrong Image";
                    break;

                case 58:                //SGFDX_ERROR_LACK_OF_BANDWIDTH  = 58,   // USB Bandwith Lack Error
                    text = "Lack of USB Bandwith";
                    break;

                case 59:                //SGFDX_ERROR_DEV_ALREADY_OPEN	= 59,   // Device Exclusive access Error
                    text = "Device is already opened";
                    break;

                case 60:                //SGFDX_ERROR_GETSN_FAILED		   = 60,   // Fail to get Device Serial Number
                    text = "Device serial number error";
                    break;

                case 61:                //SGFDX_ERROR_UNSUPPORTED_DEV		   = 61,   // Unsupported device
                    text = "Unsupported device";
                    break;

                // Extract & Verification error
                case 101:                //SGFDX_ERROR_FEAT_NUMBER		= 101, // utoo small number of minutiae
                    text = "The number of minutiae is too small";
                    break;

                case 102:                //SGFDX_ERROR_INVALID_TEMPLATE_TYPE		= 102, // wrong template type
                    text = "Template is invalid";
                    break;

                case 103:                //SGFDX_ERROR_INVALID_TEMPLATE1		= 103, // wrong template type
                    text = "1st template is invalid";
                    break;

                case 104:                //SGFDX_ERROR_INVALID_TEMPLATE2		= 104, // vwrong template type
                    text = "2nd template is invalid";
                    break;

                case 105:                //SGFDX_ERROR_EXTRACT_FAIL		= 105, // extraction fail
                    text = "Minutiae extraction failed";
                    break;

                case 106:                //SGFDX_ERROR_MATCH_FAIL		= 106, // matching  fail
                    text = "Matching failed";
                    break;

            }

            text = funcName + " Error # " + iError + " :" + text;
            
        }

    }
}
